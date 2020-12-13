using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Policies;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Hagrid.Core.Domain;
using Autofac;

namespace Hagrid.Core.Application
{
    public class CustomerImportApplication : AccountBaseApplication, ICustomerImportApplication
    {
        private ICustomerImportRepository customerImportRepository;
        private ICustomerImportService customerImportService;
        private IResetPasswordTokenRepository resetPasswordTokenRepository;
        private IApplicationStoreRepository applicationStoreRepository;
        private IPasswordLogRepository passwordLogRepository;
        private IPasswordPolicy passwordPolicy;
        private IAccountService accountService;
        private IRoleRepository roleRepository;

        public CustomerImportApplication(
            IComponentContext context,
            ICustomerImportRepository customerImportRepository, 
            ICustomerImportService customerImportService, 
            IResetPasswordTokenRepository resetPasswordTokenRepository, 
            IApplicationStoreRepository applicationStoreRepository,
            IPasswordLogRepository passwordLogRepository, 
            IPasswordPolicy passwordPolicy,
            IAccountService accountService,
            IRoleRepository roleRepository)
            : base(context)
        {
            this.customerImportRepository = customerImportRepository;
            this.customerImportService = customerImportService;
            this.resetPasswordTokenRepository = resetPasswordTokenRepository;
            this.applicationStoreRepository = applicationStoreRepository;
            this.passwordLogRepository = passwordLogRepository;
            this.passwordPolicy = passwordPolicy;
            this.accountService = accountService;
            this.roleRepository = roleRepository;
        }

        public void Save(CustomerImport customerImport)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                customerImportRepository.Save(customerImport);
            }
        }

        public bool Exists(string username, Guid storeCode)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                return customerImportService.Exists(username, storeCode);
            }
        }

        public void CreatePassword(Guid memberCode, string tokenCode, string newPassword, Guid? clientId = null)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var customerImport = customerImportRepository.Get(memberCode);

                    if (passwordPolicy == null || passwordPolicy.Validate(customerImport.Email, newPassword))
                    {
                        customerImport.Password = newPassword.Encrypt();
                        customerImport.HandleCustomer();

                        var listAppStore = new List<ApplicationStore>();
                        var applicationStore = applicationStoreRepository.GetByClientId(clientId.Value);

                        if (applicationStore.IsNull())
                            throw new ArgumentException("Aplicação inválida");

                        listAppStore.Add(applicationStore);

                        if (customerImport.StoreCode.HasValue)
                        {
                            var appsStore = applicationStoreRepository.GetByStore(customerImport.StoreCode.Value);

                            if (appsStore.IsNull())
                                throw new ArgumentException("Loja sem aplicações");

                            var appECStore = appsStore.FirstOrDefault(s => s.Application.Name.ToLower() == "ec-loja");

                            if (appECStore.IsNull())
                                throw new ArgumentException("Aplicação EC-Loja não encontrada");

                            listAppStore.Add(appECStore);
                        }
                        else
                            throw new ArgumentException("Código da loja não preenchido");

                        Customer customer;
                        if (customerImport is CompanyImport)
                            customer = new Company(customerImport);
                        else
                            customer = new Person(customerImport);

                        var _accounts = new Account()
                        {
                            Email = customerImport.Email,
                            Login = customerImport.Email,
                            Document = customerImport.DisplayDocument,
                            Password = newPassword,
                            Customer = customer,
                            Status = customerImport.Status,
                            Removed = false
                        };

                        listAppStore.ForEach(x =>
                        {
                            _accounts.ConnectApp(x);
                        });

                        Role _role;
                        listAppStore.ForEach(x =>
                        {
                            _role = roleRepository.GetByApplication(x.ApplicationCode, x.StoreCode);

                            if (!_role.IsNull())
                                _accounts.ConnectRole(_role);
                        });

                        var accountDO = accountService.Get(customerImport.Email, customerImport.DisplayDocument, applicationStore, true);

                        if (accountDO.IsNull())
                            accountService.Add(_accounts, applicationStore, customerImport.StoreCode.Value);
                        else
                            accountService.Update(_accounts);

                        customerImport.Password = string.Empty;
                        customerImport.Removed = true;
                        customerImport.UpdateDate = DateTime.Now;
                        customerImportRepository.Update(customerImport);
                    }
                    else
                    {
                        throw new ArgumentException("senha inválida.");
                    }

                    resetPasswordTokenRepository.Delete(tokenCode);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            if (clientId.HasValue && !clientId.Value.IsEmpty())
            {
                using (var transaction = Connection.BeginTransaction())
                {
                    try
                    {
                        var store = applicationStoreRepository.GetByClientId(clientId.Value).Store;

                        passwordLogRepository.Save(new PasswordLog(memberCode, PasswordEventLog.RecoveryCustomerImport, store.Code));

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public object ValidatePassword(Guid memberCode, string password)
        {
            try
            {
                var customerImport = customerImportRepository.Get(memberCode);

                if (!passwordPolicy.IsNull() && !customerImport.IsNull())
                    return passwordPolicy.Validate(customerImport.Email, password);
            }
            catch (PasswordException ex)
            {
                return ex.Issues;
            }

            return false;
        }

        public void RemoveMember(string email, string document)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    customerImportService.RemoveMember(email, document);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
