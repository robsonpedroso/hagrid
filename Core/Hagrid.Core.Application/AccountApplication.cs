using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Application
{
    public class AccountApplication : AccountBaseApplication, IAccountApplication
    {
        private readonly IEmailSender svcEmail;
        private readonly ILockedUpMemberPolicy lockedUpMemberPolicy;
        private readonly ILockMemberPolicy lockMemberPolicy;
        private readonly IPasswordPolicy passwordPolicy;
        private readonly IAccountService accountService;

        private readonly IApplicationRepository applicationRepository;
        private readonly IApplicationStoreRepository applicationStoreRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IResetPasswordTokenRepository resetPasswordTokenRepository;
        private readonly IStoreRepository storeRepositoy;
        private readonly IAccountRoleRepository accountRoleRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IResourceRepository resourceRepository;

        private readonly ICustomerImportService customerImportService;
        private readonly IResetPasswordTokenService resetPasswordTokenService;
        private readonly IApplicationStoreService applicationStoreService;
        private readonly IAccountPermissionService accPermissionService;

        private readonly IPasswordLogRepository passwordLogRepository;
        private readonly IAccountInfraService accountInfraService;
        private readonly IMetadataService metadataService;

        private readonly IResetSMSTokenService smsTokenService;

        public AccountApplication(
            IComponentContext context,
            IAccountService accountService,
            IAccountRepository accountRepository,
            IEmailSender svcEmail,
            ILockedUpMemberPolicy lockedUpMemberPolicy,
            ILockMemberPolicy lockMemberPolicy,
            IPasswordPolicy passwordPolicy,
            IApplicationStoreRepository applicationStoreRepository,
            IResetPasswordTokenRepository resetPasswordTokenRepository,
            IApplicationRepository applicationRepository,
            IStoreRepository storeRepositoy,
            IAccountRoleRepository accountRoleRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IResourceRepository resourceRepository,
            ICustomerImportService customerImportService,
            IResetPasswordTokenService resetPasswordTokenService,
            IPasswordLogRepository passwordLogRepository,
            IApplicationStoreService applicationStoreService,
            ICustomerRepository customerRepository,
            IAccountPermissionService accPermissionService,
            IAccountInfraService accountInfraService)
            : base(context)
        {
            this.svcEmail = svcEmail;
            this.lockedUpMemberPolicy = lockedUpMemberPolicy;
            this.lockMemberPolicy = lockMemberPolicy;
            this.passwordPolicy = passwordPolicy;
            this.accountService = accountService;
            this.applicationStoreRepository = applicationStoreRepository;
            this.resetPasswordTokenRepository = resetPasswordTokenRepository;
            this.applicationRepository = applicationRepository;
            this.storeRepositoy = storeRepositoy;
            this.accountRoleRepository = accountRoleRepository;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.resourceRepository = resourceRepository;
            this.passwordLogRepository = passwordLogRepository;

            this.customerImportService = customerImportService;
            this.resetPasswordTokenService = resetPasswordTokenService;
            this.applicationStoreService = applicationStoreService;
            this.accPermissionService = accPermissionService;

            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;

            if (context.TryResolveNamed(FieldType.Account.ToLower(), typeof(IMetadataService), out var metadataService))
                this.metadataService = metadataService as IMetadataService;

            this.accountInfraService = accountInfraService;
        }

        public Account Authenticate(string login, string password, ApplicationStore applicationStore, out bool requirePasswordChange, string urlBack = "")
        {
            requirePasswordChange = false;
            Account account = null;

            using (var transaction = Connection.BeginTransaction())
            {
                var accounts = accountRepository.Get(login, applicationStore, true);

                if (accounts.Count() > 0)
                {
                    var matchedAccounts = accountService.MatchPassword(accounts.ToList(), password);

                    if (matchedAccounts.IsNull() || matchedAccounts.Count() == 0)
                    {
                        if (applicationStore.Application.MemberType == MemberType.Consumer)
                        {
                            if (accounts.Any(a => !a.Customer.IsNull() && a.Customer.Code.IsEmpty()))
                            {
                                accounts.ForEach(a => a.WrongLoginAttempt(lockedUpMemberPolicy, lockMemberPolicy));
                            }
                        }
                        else
                        {
                            var _accounts = accounts.Where(a =>
                                a.AccountRoles.Any(ar =>
                                    ar.Role.Status == true &&
                                    ar.Role.Store.Status == true &&
                                    ar.Role.StoreCode == applicationStore.StoreCode &&
                                    ar.Role.Permissions.Any(p =>
                                        p.Status == true &&
                                        p.Resource.Application.Status == true &&
                                        p.Resource.ApplicationCode == applicationStore.ApplicationCode)));

                            if (!_accounts.IsNull() && _accounts.Count() > 0)
                            {
                                _accounts.ForEach(a => a.WrongLoginAttempt(lockedUpMemberPolicy, lockMemberPolicy));
                            }
                            else
                            {
                                accounts.ForEach(a => a.WrongLoginAttempt(lockedUpMemberPolicy, lockMemberPolicy));
                            }
                        }

                        transaction.Commit();

                        throw new ArgumentException("User and password not found");
                    }
                    else
                    {
                        accountService.lockedUpMemberPolicy = lockedUpMemberPolicy;
                        accountService.lockMemberPolicy = lockMemberPolicy;
                        accountService.passwordPolicy = passwordPolicy;

                        account = accountService.Authenticate(accounts, applicationStore);

                        if (passwordPolicy != null)
                        {
                            requirePasswordChange = !passwordPolicy.Validate(account.Email, password, false);
                        }

                        transaction.Commit();
                        return account;
                    }
                }
                else
                {
                    if (applicationStore.Application.MemberType == MemberType.Consumer)
                    {
                        var customerImport = customerImportService.Get(login, applicationStore.Store.Code);

                        if (customerImport != null)
                        {
                            var token = resetPasswordTokenService.GenerateResetPasswordToken(customerImport, applicationStore, urlBack);

                            var _tokenCode = token.Code.EncodeURIComponent();

                            customerImport.HandleCustomer();
                            svcEmail.SendPasswordRecoveryEmailAsync(customerImport, applicationStore.Store, _tokenCode, urlBack);

                            transaction.Commit();

                            using (var transactionLog = Connection.BeginTransaction())
                            {
                                try
                                {
                                    passwordLogRepository.Save(new PasswordLog(customerImport.AccountCode, PasswordEventLog.ResquetRecoryCustomerImport, applicationStore.Store.Code));
                                    transactionLog.Commit();
                                }
                                catch
                                {
                                    transactionLog.Rollback();
                                }
                            }

                            throw new ArgumentException("create_password_is_needed");
                        }
                    }
                }

            }

            return account;
        }

        public Account AuthenticateTransferToken(Guid accountCode, Guid clientId)
        {            
            var _account = accountRepository.Get(accountCode, true, true);

            if (_account.IsNull())
                return null;

            accountService.lockedUpMemberPolicy = lockedUpMemberPolicy;
            accountService.lockMemberPolicy = lockMemberPolicy;
            accountService.passwordPolicy = passwordPolicy;

            var applicationStore = applicationStoreRepository.GetByClientId(clientId);
            var accounts = accountRepository.Get(_account.Email, applicationStore);

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var account = accountService.Authenticate(accounts, applicationStore);

                    transaction.Commit();
                    return account;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public DTO.Account Save(DTO.Account account, Guid clientId, Guid currentAccount, bool addCurrentAppStore = true, bool simplifiedCustomer = false)
        {
            if (account.IsNull())
                throw new ArgumentException("Necessário informar os dados de usuário");

            if (account.Email.IsNullOrWhiteSpace())
                throw new ArgumentException("E-mail não informado");

            Account _account = null;
            bool convertCustomerToCompany = false;

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    ApplicationStore applicationStore = applicationStoreRepository.GetByClientId(clientId);

                    if (!applicationStore.IsNull())
                    {
                        Account accountDO = null;
                        var document = !account.Document.IsNullOrWhiteSpace() ? account.Document : null;

                        if (account.Code.IsEmpty())
                        {
                            accountDO = accountService.Get(account.Email, document, applicationStore, true, true);
                        }

                        if (account.Code.IsEmpty() && accountDO.IsNull())
                        {
                            if (account.Password.IsNullorEmpty())
                                account.Password = Account.GeneratePassword();

                            //get store code
                            var storeCode = applicationStore.GetStoreCodeByAuthType(account.StoreCode, true);

                            //get application on parameters
                            var applicationStores = applicationStoreService.Get(account.Applications, storeCode);

                            if (applicationStores.IsNull())
                                applicationStores = new List<ApplicationStore>();

                            if (addCurrentAppStore)
                                applicationStores.Add(applicationStoreService.Get(applicationStore, storeCode));

                            _account = account.Transfer(applicationStores);

                            Role _role;
                            applicationStores.ForEach(x =>
                            {
                                _role = roleRepository.GetByApplication(x.ApplicationCode, storeCode);

                                if (!_role.IsNull())
                                    _account.ConnectRole(_role);
                            });

                            Guid originStore = applicationStore.Store.IsMain ? Guid.Empty : applicationStore.Store.Code;

                            accountService.Add(_account, applicationStore, originStore, simplifiedCustomer);
                        }
                        else if (!currentAccount.IsEmpty())
                        {
                            if (!account.Code.IsEmpty())
                            {
                                var accounts = accountService.CheckEmailAndDocument(account.Email, account.Document, applicationStore, false);

                                if (accounts.Any(a => a.Code != account.Code))
                                    throw new ArgumentException("E-mail ou Docuemnto já cadastrados com outros dados");

                                _account = accountService.GetIfHasPermissionToUpdate(currentAccount, account.Code);
                            }
                            else
                            {
                                _account = accountService.GetIfHasPermissionToUpdate(currentAccount, accountDO.Code);
                            }

                            var newAccount = account.Transfer();

                            if (_account.Customer.IsNull() && !newAccount.Customer.IsNull())
                            {
                                if (newAccount.Customer.Type == CustomerType.Person)
                                {
                                    _account.Customer = new Person()
                                    {
                                        FirstName = !newAccount.Customer.Name.IsNullOrWhiteSpace() ? newAccount.Customer.Name : newAccount.Customer.Email,
                                        Email = newAccount.Customer.Email,
                                        Gender = ((Person)newAccount.Customer).Gender,
                                        Cpf = ((Person)newAccount.Customer).Cpf.ClearStrings(),
                                        Addresses = newAccount.Customer.Addresses,
                                        AddressData = newAccount.Customer.Addresses.Serialize(),
                                        Password = newAccount.Password
                                    };
                                }
                                else
                                {
                                    _account.Customer = new Company()
                                    {
                                        Cnpj = ((Company)newAccount.Customer).Cnpj.ClearStrings(),
                                        Email = newAccount.Customer.Email,
                                        Name = !newAccount.Customer.Name.IsNullOrWhiteSpace() ? newAccount.Customer.Name : newAccount.Customer.Email,
                                        Addresses = newAccount.Customer.Addresses,
                                        AddressData = newAccount.Customer.Addresses.Serialize(),
                                        Password = newAccount.Password
                                    };
                                }

                                _account.Customer = customerRepository.Save(_account.Customer);
                            }

                            accountService.Update(_account, applicationStore.Store, newAccount);
                        }
                        else
                        {
                            throw new ArgumentException("Usuário já cadastrado");
                        }


                        if (!_account.Customer.IsNull() && !_account.Code.IsEmpty() && !_account.Customer.Email.IsNullOrWhiteSpace())
                        {
                            customerImportService.RemoveMember(_account.Email, _account.Document);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Aplicação inválida");
                    }

                    account.Metadata = metadataService.SaveValue(account.GetMetadata(_account.Code)).Cast<AccountMetadata>().ToList();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    if (ex.Message == "Necessário converter customer do tipo person para company")
                    {
                        convertCustomerToCompany = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (convertCustomerToCompany && !_account.IsNull())
            {
                customerRepository.ChangeCustomerType(_account, CustomerType.Company);

                account = Save(account, clientId, currentAccount);

                return account;
            }

            return new DTO.Account(_account);
        }

        public DTO.Account SaveCustomerSimplified(DTO.Account customerSimplified, Guid clientId)
        {
            var account = Save(customerSimplified, clientId, Guid.Empty, true, true);
            return account.TransferSimplified();
        }

        public void UpdatePermission(DTO.AccountPermission accountPermission, PermissionType type, Guid clientId, Guid currentAccount)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var applicationStore = applicationStoreRepository.GetByClientId(clientId);
                    var storeCode = applicationStore.GetStoreCodeByAuthType(accountPermission.StoreCode);

                    if (!currentAccount.IsEmpty())
                    {
                        Account accountCurrent = accPermissionService.Get(currentAccount, applicationStore);

                        if (accountCurrent.IsNull())
                            throw new ArgumentException("Você não possui permissão para executar essa ação");
                    }
                    else if (!applicationStore.Store.IsMain)
                    {
                        throw new ArgumentException("Você não possui permissão para executar essa ação");
                    }

                    Account account;
                    Role role;
                    foreach (var accountCode in accountPermission.AccountCodes)
                    {
                        account = accountRepository.Get(accountCode, false, includeRole: true, includeBlacklist: true, includeApplication: true);

                        if (account.IsNull())
                            throw new ArgumentException("Usuário não encontrado");

                        if (account.BlacklistCollection.Count > 0)
                            throw new ArgumentException("Usuário na blacklist");

                        bool hasPendingChanges = false;

                        if (type == PermissionType.Add)
                        {
                            accountPermission.Applications.ForEach(applicationName =>
                            {
                                role = roleRepository.GetByApplication(applicationName, storeCode);

                                if (role.IsNull())
                                    throw new ArgumentException("Não será possível atribuir a permissão");

                                var exist = role.AccountRoles.Any(a => a.Status && a.AccountCode == account.Code && account.AccountRoles.Any(r => r.RoleCode == a.RoleCode));

                                if (!exist)
                                {
                                    account.ConnectRole(role);
                                    hasPendingChanges = true;
                                }
                            });
                        }
                        else if (type == PermissionType.Remove)
                        {
                            accountPermission.Applications.ForEach(applicationName =>
                            {
                                var accountRole = account.AccountRoles
                                    .Where(x => x.Status &&
                                           x.Role.Permissions.Any(a => a.Resource.Application.Name.ToLower() == applicationName.ToLower() &&
                                           x.Role.StoreCode == storeCode)).FirstOrDefault();

                                if (!accountRole.IsNull())
                                {
                                    accountRole.Status = false;
                                    accountRole.UpdateDate = DateTime.Now;
                                    account.AccountRoles.Append(accountRole);
                                    hasPendingChanges = true;
                                }
                            });
                        }

                        if (hasPendingChanges)
                        {
                            accountService.Update(account);
                        }
                    }
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public DTO.Account Get(Guid accountCode, Guid clientId)
        {
            var account = GetAccount(accountCode, clientId);

            if (account.IsNull())
                return null;

            return new DTO.Account(account);
        }

        public Account GetAccount(Guid accountCode, Guid clientId, bool includeRole = false)
        {
            Account account = null;
            ApplicationStore applicationStore = null;
            
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    applicationStore = applicationStoreRepository.GetByClientId(clientId);

                    account = accountService.Get(accountCode, applicationStore, includeApplication: true, includeMetadata: true, includeRole: includeRole);

                    if (!account.IsNull() && !account.Customer.IsNull())
                        account.Customer.HandlerToGet();

                    transaction.Commit();

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            if (!account.IsNull())
            {
                account.Metadata = metadataService.GetFieldAndFill(FieldType.Account, account.Metadata).Cast<AccountMetadata>().ToList();
            }

            return account;
        }

        public DTO.Account Get(string document, Guid clientId)
        {
            var app = applicationStoreRepository.GetByClientId(clientId);

            var account = accountService.Get(document, app);

            if (account.IsNull())
                return null;
            else if (!account.Customer.IsNull())
                account.Customer.HandlerToGet();

            account.Metadata = metadataService.GetFieldAndFill(FieldType.Account, account.Metadata).Cast<AccountMetadata>().ToList();

            return new DTO.Account(account);
        }

        public DTO.Account GetEmail(string email, Guid clientId)
        {
            var accounts = accountRepository.GetEmail(email, includeCustomer: true, includeMetadata: true, includeRole: true, includeApplication: true);

            if (accounts.Count() == 0)
                return null;

            var app = applicationStoreRepository.GetByClientId(clientId);

            Account account = accountService.GetAccountDuplicate(accounts, app);

            return new DTO.Account(account);
        }

        public bool IsMemberExists(DTO.AccountInput account)
        {
            var _account = account.Convert();
            return accountService.IsMemberExists(_account);
        }

        public object ValidatePassword(Guid memberCode, string password)
        {
            var account = accountRepository.Get(memberCode);

            return account.ValidatePassword(password, passwordPolicy);
        }

        public void ResetMemberPassword(Guid memberCode, string tokenCode, string newPassword, Guid? clientId = null)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var account = accountRepository.Get(memberCode);
                    account.SetPassword(newPassword, passwordPolicy);

                    if (account.QtyWrongsPassword >= Config.MaximumConsecutiveWrongLoginAttempts && account.LockedUp.HasValue)
                    {
                        account.LockedUp = null;
                        account.QtyWrongsPassword = null;
                    }

                    accountService.Update(account);

                    var token = resetPasswordTokenRepository.Get(tokenCode);
                    if (!token.IsNull())
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

                        passwordLogRepository.Save(new PasswordLog(memberCode, PasswordEventLog.Recovery, store.Code));
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void ChangeMemberPassword(Guid memberCode, string password, string newPassword, Guid? appStoreCode = null)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    accountService.lockedUpMemberPolicy = lockedUpMemberPolicy;
                    accountService.lockMemberPolicy = lockMemberPolicy;
                    accountService.passwordPolicy = passwordPolicy;

                    var account = accountRepository.Get(memberCode);
                    accountService.Authenticate(account, null, password);

                    account.SetPassword(newPassword, passwordPolicy);

                    accountService.Update(account);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            if (appStoreCode.HasValue && !appStoreCode.Value.IsEmpty())
            {
                using (var transaction = Connection.BeginTransaction())
                {
                    try
                    {
                        var appStore = applicationStoreRepository.Get(appStoreCode.Value);

                        passwordLogRepository.Save(new PasswordLog(memberCode, PasswordEventLog.Change, appStore.Store.Code));
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void ChangeAccountInfo(DTO.AccountInput input, Guid currentAccount)
        {
            var registeredAccount = accountService.GetIfHasPermissionToUpdate(currentAccount, input.Code);

            if (registeredAccount.IsNull())
                throw new ArgumentException("A conta informada não foi encontrada");

            if (!input.Email.IsNullOrWhiteSpace() && registeredAccount.Email != input.Email)
                throw new ArgumentException("O e-mail informado é diferente do e-mail registrado");

            if (!input.DocumentNew.IsNullOrWhiteSpace() && registeredAccount.Document == input.DocumentNew)
                throw new ArgumentException("O documento informado é igual o documento registrado");

            if (input.BirthdateNew.HasValue && input.BirthdateNew.Value < System.Data.SqlTypes.SqlDateTime.MinValue.AsDateTime())
                throw new ArgumentException("Data de nascimento inválida");

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    accountService.UpdateAccountInfo(registeredAccount, input.EmailNew, input.DocumentNew, input.BirthdateNew);
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void ResetAccount(DTO.AccountInput input, Guid currentAccount)
        {
            var registeredAccount = accountService.GetIfHasPermissionToUpdate(currentAccount, currentAccount);

            if (registeredAccount.IsNull())
                throw new ArgumentException("A conta informada não foi encontrada");

            if (!input.EmailNew.IsValidEmail())
                throw new ArgumentException("O e-mail informado não é válido");

            if (input.EmailNew.IsNullorEmpty())
                throw new ArgumentException("O e-mail não foi informado");

            if (input.PasswordNew.IsNullorEmpty())
                throw new ArgumentException("A senha não foi informada");

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    registeredAccount.SetPassword(input.PasswordNew, passwordPolicy);
                    accountService.UpdateAccountInfo(registeredAccount, email: input.EmailNew);
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public object AccountsAdminTransferToken(string transferToken)
        {
            return TransferToken(transferToken, Config.AccountAdminClientId, Config.AccountAdminSecret);
        }

        public object CustomerDashboardTransferToken(string transferToken)
        {
            return TransferToken(transferToken, Config.CustomerDashboardClientId, Config.CustomerDashboardSecret);
        }

        public object TransferToken(string transferToken, Guid clientId, string secret)
        {
            if (transferToken.IsNullorEmpty())
                throw new ArgumentException("Transfer Token inválido");

            var token = accountInfraService.GetToken(transferToken, clientId, secret);

            if (token.IsNull())
                throw new ArgumentException();

            Account account = null;

            var applicationStore = applicationStoreRepository.GetByClientId(clientId);

            if (applicationStore.IsNull())
                throw new ArgumentException("Transfer Token inválido - Aplicação não encontrada");

            account = accPermissionService.Get(token.AccountCode, applicationStore);

            if (account.IsNull())
                throw new ForbiddenException();

            return new
            {
                code = account.Code,
                email = account.Email,
                token = new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                }
            };
        }

        public object CustomerDashboardChangePasswordToken(string token)
        {
            if (token.IsNullorEmpty())
                throw new ArgumentException("Token inválido");

            return accountInfraService.GetChangePasswordToken(token, Config.CustomerDashboardClientId, Config.CustomerDashboardSecret);
        }

        public object CustomerDashboardChangePassword(DTO.ChangePassword changePassword)
        {
            return accountInfraService.ChangePassword(changePassword.Token, changePassword.password, changePassword.passwordNew, Config.CustomerDashboardClientId, Config.CustomerDashboardSecret);
        }

        public DTO.SearchResult Search(DTO.SearchFilter filter)
        {
            var _filter = filter as DTO.SearchFilterAccount;

            if (_filter.Code.IsEmpty() && _filter.Document.IsNullorEmpty() && _filter.Email.IsNullorEmpty())
                return null;

            _filter.Email = _filter.Email.AsString("").Trim();
            _filter.Document = _filter.Document.ClearStrings().AsString("").Trim();

            var result = accountRepository.Search(_filter);
            var accounts = result.Results.Select(a => new DTO.Account(a));

            return new DTO.SearchResult(accounts).SetResult<Account>(result);
        }

        public DTO.Account UnlockUser(Guid code, Guid currentAccount)
        {
            var account = accountService.GetIfHasPermissionToUpdate(currentAccount, code);

            if (!account.IsNull() && !account.Customer.IsNull())
            {
                account.Customer.HandlerToGet();
            }

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    account = accountService.UnlockUser(account);
                }
                catch
                {
                    transaction.Rollback();

                    throw new ArgumentException("Não foi possível realizar essa operação nesse momento, tente mais tarde novamente.");
                }
            }

            var _account = new DTO.Account(account, account.AccountApplicationStoreCollection.Select(a => a.ApplicationStore).ToList());

            return _account;
        }

        public void Remove(Guid accountCode, Guid currentAccount)
        {
            var registeredAccount = accountService.GetIfHasPermissionToUpdate(currentAccount, accountCode, false, true);

            if (registeredAccount.IsNull())
                throw new ArgumentException("Você não possui permissão para executar essa ação.");

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    if (registeredAccount.AccountRoles.Count > 0)
                    {
                        foreach (var accRole in registeredAccount.AccountRoles.ToList())
                        {
                            accountRoleRepository.Delete(accRole.Code);
                        }
                    }

                    registeredAccount.SetRemoved();

                    accountRepository.Update(registeredAccount);
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public DTO.Account GetDetail(Guid accountCode, Guid currentAccount, string accountEmail, bool blacklist = false, bool role = false)
        {
            var account = accountService.GetIfHasPermissionToUpdate(currentAccount, accountCode, blackList: blacklist, role: role, metaData: true);

            if (account.IsNull())
                throw new ArgumentException("Usuário não encontrado");

            if (!account.IsNull() && !account.Customer.IsNull())
                account.Customer.HandlerToGet();

            account.Metadata = metadataService.GetFieldAndFill(FieldType.Account, account.Metadata).Cast<AccountMetadata>().ToList();

            if (Config.PermissionsShowConfSecret.Contains(accountEmail))
                return new DTO.Account(account, account.AccountApplicationStoreCollection.Select(a => a.ApplicationStore).ToList(), blacklist);
            else
                return new DTO.Account(account, null, blacklist);
        }

        #region "  Address   "

        public void RemoveAddress(Guid accountCode, Guid addressCustomerCode)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    accountService.RemoveAddress(accountCode, addressCustomerCode);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public DTO.Address Save(DTO.Address address, Guid accountCode)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    if (!address.IsNull())
                    {
                        var newAddress = address.Transfer();

                        var _address = accountService.SaveOrUpdateAddress(newAddress, accountCode) as VO.AddressCustomer;

                        address.AddressCustomerCode = _address.AddressCustomerCode;
                    }
                    else
                    {
                        throw new ArgumentException("É preciso preencher os dados do endereço.");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return address;
        }

        public DTO.Address GetByCode(Guid accountCode, Guid addressCustomerCode)
        {
            var address = accountService.GetByCode(accountCode, addressCustomerCode);

            if (address.IsNull())
                return null;

            return new DTO.Address(address as AddressCustomer);
        }

        public List<DTO.Address> GetAdresses(Guid code)
        {
            var addressesResult = accountService.GetAdresses(code);

            if (addressesResult.IsNull())
                return null;

            return addressesResult.Select(a => new DTO.Address(a)).ToList();
        }

        public DTO.ResetSMSToken GetTokenSMS(DTO.AccountInput accountInput, Guid clientId)
        {
            Account account = null;
            ResetSMSToken token = null;

            var applicationStore = applicationStoreRepository.GetByClientId(clientId);

            if (accountInput.Document.IsValidCNPJ() || accountInput.Document.IsValidCPF())
                account = accountService.Get(accountInput.Document, applicationStore);

            else if (accountInput.Email.IsValidEmail())
                account = accountService.Get(accountInput.Email, applicationStore);

            if (account.IsNull())
                throw new ArgumentException("Cliente não encontrado");
            else
            {
                if (!account.IsNull() && !account.Customer.IsNull())
                    account.Customer.HandlerToGet();
                else
                    throw new ArgumentException("Não foram encontrados os dados de consumidor do cliente");

                using (var transaction = Connection.BeginTransaction())
                {
                    try
                    {
                        token = smsTokenService.Generate(account.Customer, applicationStore, accountInput.UrlBack);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return new DTO.ResetSMSToken(token);
        }

        #endregion

        #region " Account Permission  "

        public void LinkAccountRole(Guid accountCode, Guid roleCode, bool isRKAdmin, Guid clientId)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var _accountRole = ValidateField(accountCode, roleCode, isRKAdmin, clientId);

                    if (!_accountRole.IsNull())
                    {
                        _accountRole.Status = true;
                        _accountRole.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        _accountRole = new AccountRole()
                        {
                            Code = Guid.NewGuid(),
                            AccountCode = accountCode,
                            RoleCode = roleCode,
                            Status = true,
                        };
                    }

                    accountRoleRepository.Save(_accountRole);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UnLinkAccountRole(Guid accountCode, Guid roleCode, bool isRKAdmin, Guid clientId)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var _accountRole = ValidateField(accountCode, roleCode, isRKAdmin, clientId);

                    if (!_accountRole.IsNull())
                        accountRoleRepository.Delete(_accountRole);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<DTO.Role> GetRole(Guid accountCode, Guid clientId)
        {
            var appStore = applicationStoreRepository.GetByClientId(clientId);

            if (appStore.IsNull())
                throw new ArgumentException("Aplicação não encontrada");

            var role = roleRepository.ListRoles(appStore.StoreCode, appStore.ApplicationCode, accountCode);

            return role.Select(r => new DTO.Role(r, withPermission:true, withAccountRole:false)).ToList();
        }

        private AccountRole ValidateField(Guid accountCode, Guid roleCode, bool isRKAdmin, Guid clientId)
        {
            try
            {
                ApplicationStore appStore;
                AccountRole _accountRole;

                if (accountCode.IsNull())
                    throw new ArgumentException("Código da conta não informado");

                if (roleCode.IsNull())
                    throw new ArgumentException("Código do grupo não informado");

                var _role = roleRepository.Get(roleCode, null);

                if (_role.IsNull())
                    throw new ArgumentException("Grupo não encontrado");

                var _account = accountRepository.Get(accountCode);

                if (_account.IsNull())
                    throw new ArgumentException("Usuário não encontrada");

                _accountRole = accountRoleRepository.Get(accountCode, roleCode);

                if (!isRKAdmin)
                {
                    appStore = applicationStoreRepository.GetByClientId(clientId);

                    if (appStore.IsNull())
                        throw new ArgumentException("Credenciais inválidas");

                    var _permission = permissionRepository.GetPermission(_role.Code);

                    if (_permission.IsNull())
                        throw new ArgumentException("Permissão não encontrada");

                    if (appStore.ApplicationCode != _permission.Resource.ApplicationCode)
                        throw new ArgumentException("Não é possível realizar essa operação.");
                }

                return _accountRole;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}