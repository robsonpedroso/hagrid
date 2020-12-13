using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using Hagrid.Infra.Utils;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using Hagrid.Core.Domain.ValueObjects;
using System.Threading;

namespace Hagrid.Core.Domain.Services.Requisitions
{
    public class CustomerImportDBService : IRequisitionProcessingService
    {
        private IRequisitionRepository requisitionRepository;
        private ICustomerImportDBInfraService customerImportDBInfraService;
        private IAccountService accountService;
        private IApplicationStoreRepository applicationStoreRepository;
        private ICustomerImportService customerImportService;
        private IRoleRepository roleRepository;

        public CustomerImportDBService(IRequisitionRepository requisitionRepository,
            ICustomerImportDBInfraService customerImportDBInfraService,
            IAccountService accountService,
            IApplicationStoreRepository applicationStoreRepository,
            ICustomerImportService customerImportService,
            IRoleRepository roleRepository)
        {
            this.requisitionRepository = requisitionRepository;
            this.customerImportDBInfraService = customerImportDBInfraService;
            this.accountService = accountService;
            this.applicationStoreRepository = applicationStoreRepository;
            this.customerImportService = customerImportService;
            this.roleRepository = roleRepository;
        }

        public void ImportCustomerToImportDb(Requisition requisition)
        {
            if (Config.ProcessImportCustomerFull)
            {
                if (!customerImportDBInfraService.ImportCustomer(requisition))
                    requisition.Status = RequisitionStatus.Failure;
            }
        }

        public void ClearImportDb(Requisition requisition)
        {
            customerImportDBInfraService.Clear(requisition);
        }

        public ApplicationStore GetApplicationStore(Guid storeCode)
        {
            var appStores = applicationStoreRepository.GetByStore(storeCode);

            if (appStores.IsNull())
                throw new ArgumentException("Essa loja não tem aplicações, o código da loja esta correto?");

            var appStore = appStores.FirstOrDefault(s => s.Application.Name == "EC-Loja");

            if (appStore.IsNull())
                throw new ArgumentException("Essa loja não tem aplicação do tipo EC-Loja, o código da loja esta correto?");

            return appStore;
        }

        public object[] GetAccounts(Requisition requisition, int skip, int take)
        {
            var conn = requisitionRepository.Connection;
            accountService.GetRepositories().ForEach(r => r.Connection = conn);
            customerImportService.GetRepositories().ForEach(r => r.Connection = conn);

            return customerImportDBInfraService.GetCustomers(requisition, skip, Config.ProcessImportNumberRecordsPerCommit).ToArray();
        }


        public bool SaveAccount(Requisition requisition, Object account, ApplicationStore applicationStore, IConnection conn)
        {
            var acc = (DTO.Account)account;

            // Set Connection
            accountService.GetRepositories().ForEach(r => r.Connection = conn);
            customerImportService.GetRepositories().ForEach(r => r.Connection = conn);

            var messageError = string.Empty;
            var isValid = true;
            try
            {
                var accountDO = accountService.Get(acc.Email, acc.Document, applicationStore, true);

                if (accountDO.IsNull())
                {
                    var _account = acc.Transfer();

                    _account.AccountApplicationStoreCollection = new List<AccountApplicationStore>() { new AccountApplicationStore(_account.Code, applicationStoreRepository.Get(applicationStore.Code).Code) };

                    var role = roleRepository.GetByApplication(applicationStore.ApplicationCode, applicationStore.StoreCode);
                    if (!role.IsNull())
                        _account.ConnectRole(role);

                    accountService.Add(_account, applicationStore, requisition.Store.Code);

                    if (!_account.Customer.IsNull() && !_account.Customer.Email.IsNullOrWhiteSpace())
                        customerImportService.RemoveMember(_account.Email, _account.Document);

                }
                else
                {
                    isValid = false;
                    messageError = "Usuário já cadastrado";
                }
            }
            catch (Exception ex)
            {
                isValid = false;

                if (!ex.InnerException.IsNull() && !ex.InnerException.Message.IsNullorEmpty())
                    messageError = ex.InnerException.Message;
                else
                    messageError = ex.Message;
            }

            if (!isValid)
            {
                requisition.RequisitionErrors.Add(new RequisitionError()
                {
                    Email = acc.Login,
                    Name = acc.Customer.Name,
                    Line = 0,
                    ErrorMessages = new List<string>() { messageError }
                });
            }

            return isValid;
        }



        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                requisitionRepository,
                applicationStoreRepository,
                roleRepository
            };
        }

        #endregion
    }
}
