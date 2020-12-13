using Hagrid.Core.Domain.Contracts.Factories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ExceptionObjects;
using Hagrid.Core.Domain.Policies;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Logging.Slack;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerImportService customerImportService;
        private readonly IAccountApplicationStoreService accountApplicationStoreService;

        private readonly IAccountRepository accountRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IApplicationStoreRepository applicationStoreRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IRoleRepository roleRepository;

        private readonly IResetPasswordTokenFactory resetPasswordTokenFactory;
        private readonly IEmailSender svcEmail;
        private readonly IAccountPermissionService accPermissionService;

        public ILockedUpMemberPolicy lockedUpMemberPolicy { get; set; }
        public ILockMemberPolicy lockMemberPolicy { get; set; }
        public IPasswordPolicy passwordPolicy { get; set; }

        private SlackMessager<AccountService> slack;

        public AccountService(ICustomerService customerService,
            ICustomerImportService customerImportService,
            IAccountApplicationStoreService accountApplicationStoreService,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IApplicationStoreRepository applicationStoreRepository,
            IApplicationRepository applicationRepository,
            IResetPasswordTokenFactory resetPasswordTokenFactory,
            IEmailSender svcEmail,
            IPasswordPolicy passwordPolicy,
            IAccountPermissionService accPermissionService,
            IRoleRepository roleRepository,
            ILockedUpMemberPolicy lockedUpMemberPolicy)
        {
            this.customerService = customerService;
            this.customerImportService = customerImportService;
            this.accountApplicationStoreService = accountApplicationStoreService;
            this.accPermissionService = accPermissionService;
            this.roleRepository = roleRepository;

            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
            this.applicationStoreRepository = applicationStoreRepository;
            this.applicationRepository = applicationRepository;
            this.resetPasswordTokenFactory = resetPasswordTokenFactory;
            this.svcEmail = svcEmail;
            this.passwordPolicy = passwordPolicy;

            this.lockedUpMemberPolicy = lockedUpMemberPolicy;

            slack = new SlackMessager<AccountService>();
        }


        public void Add(Account account, ApplicationStore applicationStore, Guid originStore, bool simplifiedCustomer = false)
        {
            if (applicationStore.IsNull())
                throw new ArgumentException("Applicação inválida");

            account.IsValid(simplifiedCustomer);

            account.SetPassword(account.Password, passwordPolicy);

            if (simplifiedCustomer || !account.Customer.IsNull())
            {

                var customer = account.Customer;
                customer.Password = account.Password;
                customer.Account = account;

                if (!simplifiedCustomer && !account.Document.IsNullorEmpty())
                    customerService.PrepareToAdd(customer, originStore);
                else
                    customerService.PrepareToAddSimplified(customer);
            }

            account.SaveDate = DateTime.Now;
            account.UpdateDate = DateTime.Now;

            var _account = accountRepository.Save(account);

            if (!account.CodeEmailTemplate.IsNull())
            {
                var resetPasswordTokenService = resetPasswordTokenFactory.GetResetPasswordTokenService(_account);
                SetConnection(resetPasswordTokenService);

                resetPasswordTokenService.lockedUpMemberPolicy = lockedUpMemberPolicy;

                var token = resetPasswordTokenService.GenerateResetPasswordToken(_account, applicationStore, "");

                var _tokenCode = token.Code.EncodeURIComponent();

                svcEmail.SendEmailUserCreatedByAccountAdminAsync(_account, _tokenCode, "");
            }
        }

        public void Update(Account registeredAccount, Store currentStore = null, Account newAccount = null)
        {
            registeredAccount.UpdateDate = DateTime.Now;

            if (!newAccount.IsNull())
            {
                registeredAccount.Transfer(newAccount);

                if (!newAccount.Customer.IsNull())
                {
                    if (registeredAccount.Customer is Person && newAccount.Customer is Company)
                        throw new ArgumentException("Necessário converter customer do tipo person para company");

                    if (registeredAccount.Customer is Person registeredPerson && newAccount.Customer is Person newPerson)
                    {
                        if (!registeredPerson.BirthDate.HasValue)
                            newPerson.BirthDate = newPerson.BirthDate ?? registeredPerson.BirthDate;
                    }

                    registeredAccount.Customer = customerService.PrepareToUpdate(currentStore, registeredAccount.Customer, newAccount.Customer);
                }
            }

            accountRepository.Update(registeredAccount);
        }

        public Account Get(string login, ApplicationStore applicationStore, string password = "")
        {
            Account account = accPermissionService.Get(login, applicationStore).FirstOrDefault();
            Account _account = null;

            if (!password.IsNullOrWhiteSpace() && !account.IsNull() && !account.Password.Equals(password.Encrypt()))
            {
                _account = account;
                account = null;
            }

            //dont has connect
            if (account.IsNull() &&
                (applicationStore.Application.MemberType == Domain.Enums.MemberType.Consumer || applicationStore.Application.Name.ToLower() == "mp-accounts"))
            {
                var accounts = accPermissionService.Get(login, applicationStore);

                account = CreateConnect(accounts, applicationStore, password);

                if (!password.IsNullOrWhiteSpace() && accounts.Count() > 0 && account.IsNull())
                {
                    accounts.ToList().ForEach(acc =>
                    {
                        if (acc.Code != _account.Code && lockedUpMemberPolicy.Validate(acc, false) && lockedUpMemberPolicy.Validate(acc, applicationStore, false))
                        {
                            acc.WrongLoginAttempt(lockedUpMemberPolicy, lockMemberPolicy);
                            Update(acc);
                        }
                    });
                }
            }

            if (account.IsNull() && !_account.IsNull())
            {
                account = _account;
            }

            return account;
        }

        public Account Get(string email, string document, ApplicationStore applicationStore, bool checkEmailAndDocument = false, bool isSave = false)
        {
            List<Account> accounts = new List<Account>();

            var listEmail = accountRepository.Get(email, null, null).ToList();
            accounts.AddRange(listEmail);
            
            if (!document.IsNullOrWhiteSpace()) { 
                var listDocument = accountRepository.Get(document, null, null).ToList();
                accounts.AddRange(listDocument);
            }

            var account = CreateConnect(accounts, applicationStore, string.Empty, isSave);

            if (checkEmailAndDocument && ((!account.IsNull() && !account.Document.IsNullOrWhiteSpace()) || account.IsNull()))
                CheckEmailAndDocument(email, document, applicationStore, true);

            if (!account.IsNull() && !account.Customer.IsNull())   
            {
                account.Customer.HandlerToGet();
            }

            return account;
        }

        public IEnumerable<Account> CheckEmailAndDocument(string email, string document, ApplicationStore applicationStore, bool throwException)
        {
            List<Account> accounts = accountRepository.Get(email, null, null).ToList();

            accounts.ForEach(account =>
            {
                if (account.Document != document && throwException)
                {
                    throw new ArgumentException("E-mail, CPF ou CNPJ já cadastrados com outros dados");
                }
            });

            if (!document.IsNullOrWhiteSpace())
            {
                var accountsDocument = accountRepository.Get(document, null, null).ToList();

                accountsDocument.ForEach(account =>
                {
                    if (account.Email != email && throwException)
                    {
                        throw new ArgumentException("E-mail, CPF ou CNPJ já cadastrados com outros dados");
                    }
                });

                if (accountsDocument.Count() > 0)
                    accounts.AddRange(accountsDocument);

            }

            return accounts;
        }

        public Account Get(Guid accountCode, ApplicationStore applicationStore, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            Account account = accPermissionService.Get(accountCode, applicationStore, includeMetadata: includeMetadata, includeRole: includeRole, includeBlacklist: includeBlacklist, includeApplication: includeApplication);

            return account;
        }

        public Account GetIfHasPermissionToUpdate(Guid currentAccount, Guid accountCodeToUpdate, bool blackList = false, bool role = false, bool metaData = false)
        {
            return accPermissionService.Get(currentAccount, accountCodeToUpdate, includeMetadata: metaData, includeBlacklist: blackList, includeRole: role);
        }

        public bool IsMemberExists(Account account, bool withDifferentAccountCode = false)
        {
            return accountRepository.IsMemberExists(account, withDifferentAccountCode);
        }

        public Account Authenticate(IEnumerable<Account> accounts, ApplicationStore applicationStore, bool unlockAccount = true)
        {
            Account account = null;
            IEnumerable<Account> _acc = null;

            if (applicationStore.Application.MemberType == MemberType.Consumer)
            {
                _acc = accountRepository.GetOffBlacklist(accounts.Select(a => a.Code), applicationStore.StoreCode);

                if (_acc.IsNull() || _acc.Count() == 0)
                    throw new AccountNotFoundException("Usuário não existe");
            }
            else if(applicationStore.Application.AuthType == AuthType.Distributed)
            {
                _acc = accountRepository.GetOffBlacklistAndHasPermission(accounts.Select(a => a.Code), applicationStore.StoreCode, applicationStore.ApplicationCode);
            }
            else if(applicationStore.Store.IsMain)
            {
                _acc = accountRepository.GetOffBlacklistAndHasPermissionUnified(accounts.Select(a => a.Code), applicationStore.StoreCode, applicationStore.ApplicationCode);
            }
            else
            {
                _acc = null;
            }

            if (_acc.IsNull() || _acc.Count() == 0)
            {
                throw new AccountNotFoundException("Usuário não possui permissão para acessar esta loja ou aplicação");
            }
            else
            {
                var _accounts = _acc.Where(a => lockedUpMemberPolicy.Validate(a, false));

                if (_accounts.IsNull() || _accounts.Count() == 0)
                {
                    throw new LockedUpMemberException(_acc.FirstOrDefault());
                }
                else if (_accounts.Count() > 1)
                {
                    account = GetAccountDuplicate(_acc, applicationStore);

                    string messageFlow = unlockAccount ? "account recovery" : "login";

                    slack.Warn($"Duplicate {messageFlow} password information for IDs: {string.Join(";", _acc.Select(a => a.Code.ToString()).ToArray<string>())}");
                }
                else
                {
                    account = _accounts.FirstOrDefault();
                }

                account.SuccessfullyLogin(lockedUpMemberPolicy, lockMemberPolicy);
                Update(account);
                SetConnection(accountApplicationStoreService);
                accountApplicationStoreService.Save(new AccountApplicationStore(account.Code, applicationStore.Code));

                return account;
            }
        }

        public void Authenticate(Account account, ApplicationStore applicationStore, string password)
        {
            if (!account.IsNull() && account.Status && !account.Removed)
            {
                if (lockedUpMemberPolicy == null || (lockedUpMemberPolicy.Validate(account) && (applicationStore.IsNull() || lockedUpMemberPolicy.Validate(account, applicationStore))))
                {
                    if (!password.IsNullOrWhiteSpace() && account.Password.Equals(password.Encrypt()))
                    {
                        account.SuccessfullyLogin(lockedUpMemberPolicy, lockMemberPolicy);
                        Update(account);
                    }
                    else
                    {
                        account.WrongLoginAttempt(lockedUpMemberPolicy, lockMemberPolicy);
                        Update(account);

                        throw new ArgumentException("invalid_password");
                    }
                }
            }
            else
                throw new ArgumentException("invalid_account");
        }

        public void UpdateAccountInfo(Account account, string email = null, string document = null, DateTime? birthdate = null)
        {
            var updatedAccount = new Account
            {
                Code = account.Code,
                Email = email ?? account.Email,
                Login = email ?? account.Email,
                Document = document ?? account.Document
            };

            if (accountRepository.IsMemberExists(updatedAccount, withDifferentAccountCode: true))
                throw new ArgumentException("E-mail ou Documento já cadastrados com outros dados");

            if (!email.IsNullOrWhiteSpace())
                account.ChangeEmail(email);

            if (!document.IsNullOrWhiteSpace())
            {
                if ((account.Customer is Person && !document.IsValidCPF()) || (account.Customer is Company && !document.IsValidCNPJ()))
                    throw new ArgumentException("Documento inválido");

                account.ChangeDocument(document);
            }

            if (birthdate.HasValue)
            {
                if (account.Customer.Type == CustomerType.Company)
                    throw new ArgumentException("Não é possivel alterar data de nascimento de pessoas juridicas");

                account.ChangeBirthdate(birthdate.Value);
            }

            Update(account);
        }

        public Account GetAccountDuplicate(IEnumerable<Account> accounts, ApplicationStore app)
        {
            Account account = null;

            if (app.Application.MemberType == MemberType.Consumer)
                account = accounts.FirstOrDefault(a => !a.Customer.IsNull());
            else
            {
                account = accounts.Where(a => a.AccountRoles.Any(ar =>
                ar.Status &&
                ar.Role.Status &&
                ar.Role.Permissions.Any(p => 
                    p.Status &&
                    p.Resource.Application.Status &&
                    p.Resource.ApplicationCode == app.ApplicationCode)

                )).FirstOrDefault();
            }

            if (account.IsNull())
                account = accounts.OrderBy(a => a.SaveDate).FirstOrDefault();

            return account;
        }

        #region " UnLock user "

        public Account UnlockUser(Account account)
        {
            if (account.IsNull())
                throw new ArgumentException("Hum, é necessário enviar as informações do usuário.");

            account.QtyWrongsPassword = null;
            account.LockedUp = null;
            account.UpdateDate = DateTime.Now;

            Update(account);

            return account;
        }

        #endregion

        private Account CreateConnect(IEnumerable<Account> accounts, ApplicationStore applicationStore, string password = "", bool isSave = false)
        {
            Account account = null;

            if (!accounts.IsNull() && accounts.Count() > 0)
            {
                for (int i = 0; i < accounts.Count(); i++)
                {
                    account = accounts.ElementAt(i);

                    if ((!password.IsNullOrWhiteSpace() && account.Password.Equals(password.Encrypt())) ||
                        (password.IsNullOrWhiteSpace() && applicationStore.Application.MemberType == Domain.Enums.MemberType.Consumer &&
                            (!account.Document.IsNullOrWhiteSpace() || !account.Customer.IsNull())))
                    {
                        //connect account to store
                        var role = roleRepository.GetByApplication(applicationStore.ApplicationCode, applicationStore.StoreCode);
                        if (!role.IsNull())
                            account.ConnectRole(role);

                        //connect account to store
                        var app = applicationStoreRepository.Get(applicationStore.Code);
                        account.ConnectApp(app);

                        break;
                    }
                    else if (account.AccountApplicationStoreCollection.Any(appSto => appSto.ApplicationStore.Application != null && appSto.ApplicationStore.Application.MemberType == MemberType.Merchant) && isSave)
                    {
                        throw new ArgumentException("E-mail já cadastrado");
                    }
                    else
                    {
                        account = null;
                    }
                }
            }

            return account;
        }

        #region "  Address  "

        public void RemoveAddress(Guid accountCode, Guid addressCustomerCode)
        {
            List<VO.AddressCustomer> resultAddress;

            if (accountCode.IsEmpty())
            {
                throw new ArgumentException("É necessário informar o código da conta.");
            }

            if (addressCustomerCode.IsEmpty())
            {
                throw new ArgumentException("É necessário informar o código do endereço.");
            }

            var account = accountRepository.Get(accountCode, true);

            if (!account.IsNull() && !account.Customer.IsNull())
            {
                resultAddress = account.Customer.AddressData.Deserialize<List<VO.AddressCustomer>>();

                if (resultAddress.Count == 1)
                {
                    throw new ArgumentException("Não é possível excluir o único endereço salvo.");
                }

                if (resultAddress.Any(a => a.AddressCustomerCode == addressCustomerCode && a.Purpose == AddressPurposeType.Contact))
                    throw new ArgumentException("O endereço de contato não pode ser removido.");

                resultAddress.RemoveAll(a => a.AddressCustomerCode == addressCustomerCode);

                account.Customer.AddressData = resultAddress.Serialize<List<VO.AddressCustomer>>().RemoveHeaderXML();

                customerRepository.Save(account.Customer);
            }
            else
            {
                throw new ArgumentException("Dados de endereço não localizado.");
            }
        }

        public VO.AddressCustomer SaveOrUpdateAddress(VO.AddressCustomer address, Guid accountCode)
        {
            address.IsValid();

            if (address.Purpose == AddressPurposeType.None || address.Purpose.IsNull())
            {
                address.Purpose = AddressPurposeType.Shipping;
            }

            address.Country = address.Country.IsNullOrWhiteSpace() ? "Brasil" : address.Country;

            address.Status = address.Status.IsNull() ? true : address.Status;
            var account = accountRepository.Get(accountCode, true);

            if (account.IsNull())
                return new VO.AddressCustomer();

            Entities.Customer _customer = account.Customer;

            if (!_customer.IsNull())
            {
                List<AddressCustomer> resultAdresses = _customer.AddressData.Deserialize<List<VO.AddressCustomer>>();

                var addressOld = resultAdresses.FirstOrDefault(a => a.AddressCustomerCode == address.AddressCustomerCode);

                if (addressOld.IsNull())
                {
                    if (!address.AddressCustomerCode.IsEmpty())
                        throw new ArgumentException("Endereço não encontrado com esse código: {0}".ToFormat(address.AddressCustomerCode));

                    addressOld = resultAdresses.FirstOrDefault(a => a.Number.ToLower() == address.Number.ToLower() && a.ZipCode.ClearStrings() == address.ZipCode.ClearStrings());
                }

                if (addressOld.IsNull())
                {
                    address.AddressCustomerCode = Guid.NewGuid();
                    address.UpdateDate = DateTime.Now;
                    resultAdresses.Add(address);
                }
                else
                {
                    resultAdresses.ForEach(a =>
                    {
                        if (a.Number.ToLower() == address.Number.ToLower() && a.ZipCode.ClearStrings() == address.ZipCode.ClearStrings() || addressOld.AddressCustomerCode == address.AddressCustomerCode)
                        {
                            a.AddressCustomerCode = a.AddressCustomerCode;
                            a.Name = address.Name;
                            a.ContactName = address.ContactName;
                            a.Type = address.Type;
                            a.Street = address.Street;
                            a.Complement = address.Complement;
                            a.District = address.District;
                            a.City = address.City;
                            a.ZipCode = address.ZipCode;
                            a.Number = address.Number;
                            a.State = address.State;
                            a.Country = address.Country;
                            a.Purpose = address.Purpose;
                            a.Phones = address.Phones;
                            a.UpdateDate = DateTime.Now;
                            a.SaveDate = a.SaveDate;
                        }
                    });
                }

                _customer.AddressData = resultAdresses.Serialize<List<AddressCustomer>>().RemoveHeaderXML();

                _customer.UpdateDate = DateTime.Now;

                customerRepository.Save(_customer);

                address.AddressCustomerCode = !address.AddressCustomerCode.IsEmpty() ? address.AddressCustomerCode : addressOld.AddressCustomerCode;

                return address;
            }
            return new VO.AddressCustomer();
        }

        public VO.AddressCustomer GetByCode(Guid accountCode, Guid addressCustomerCode)
        {
            List<VO.AddressCustomer> adresses;

            if (accountCode.IsEmpty() || addressCustomerCode.IsEmpty())
            {
                throw new ArgumentException("Informe o código corretamente.");
            }

            var account = accountRepository.Get(accountCode, true);

            if (account.IsNull() || account.Customer.IsNull())
                return null;

            if (account.Customer.AddressData.IsNull())
                return null;

            adresses = account.Customer.AddressData.Deserialize<List<VO.AddressCustomer>>();

            return adresses.FirstOrDefault(a => a.AddressCustomerCode == addressCustomerCode);
        }

        public List<VO.AddressCustomer> GetAdresses(Guid code)
        {
            List<VO.AddressCustomer> adresses;

            if (code.IsNull())
            {
                throw new ArgumentException("É necessário informar o código do consumidor.");
            }

            var account = accountRepository.Get(code, true);

            if (account.IsNull() || account.Customer.IsNull())
                return null;

            if (account.Customer.AddressData.IsNull())
                return null;

            return adresses = account.Customer.AddressData.Deserialize<List<VO.AddressCustomer>>();
        }

        #endregion

        public IEnumerable<Account> MatchPassword(IEnumerable<Account> accounts, string password)
        {
            return accounts.Where(a => a.Password == password.Encrypt());
        }


        private void SetConnection(Infra.Contracts.IDomainService domainService)
        {
            var conn = accountRepository.Connection;
            domainService.GetRepositories().ForEach(r => r.Connection = conn);
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            var serviesRepository = new List<IRepository>();
            serviesRepository.AddRange(accPermissionService.GetRepositories());
            
            return new List<IRepository>(serviesRepository) {
                accountRepository,
                customerRepository,
                applicationStoreRepository,
                applicationRepository,
                roleRepository
            };
        }

        #endregion
    }
}