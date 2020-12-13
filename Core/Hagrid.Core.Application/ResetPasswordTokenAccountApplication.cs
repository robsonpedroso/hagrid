using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Factories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Application
{
    public class ResetPasswordTokenAccountApplication : ResetPasswordTokenApplication, IAccountResetPasswordTokenApplication
    {
        private readonly IResetPasswordTokenFactory resetPasswordTokenFactory;
        private readonly IAccountService accountService;
        private readonly ICustomerImportService customerImportService;

        private readonly IApplicationStoreRepository applicationStoreRepository;
        private readonly IPasswordLogRepository passwordLogRepository;
        
        private readonly IEmailSender svcEmail;
        private readonly ILockedUpMemberPolicy lockedUpMemberPolicy;
        
        private readonly ILockMemberPolicy lockMemberPolicy;
        private readonly IPasswordPolicy passwordPolicy;
        private readonly IAccountRepository accountRepository;

        public ResetPasswordTokenAccountApplication(
            IComponentContext context,
            IResetPasswordTokenFactory resetPasswordTokenFactory,
            IAccountService accountService,
            ICustomerImportService customerImportService,
            IResetPasswordTokenRepository resetPasswordTokenRepository,
            IApplicationStoreRepository applicationStoreRepository,
            IPasswordLogRepository passwordLogRepository,
            IEmailSender svcEmail,
            ILockedUpMemberPolicy lockedUpMemberPolicy,
            ILockMemberPolicy lockMemberPolicy,
            IPasswordPolicy passwordPolicy,
            IAccountRepository accountRepository)
            : base(context, resetPasswordTokenRepository, applicationStoreRepository)
        {
            this.resetPasswordTokenFactory = resetPasswordTokenFactory;
            this.accountService = accountService;
            this.customerImportService = customerImportService;
            this.applicationStoreRepository = applicationStoreRepository;
            this.passwordLogRepository = passwordLogRepository;
            this.svcEmail = svcEmail;
            this.lockedUpMemberPolicy = lockedUpMemberPolicy;
            this.lockMemberPolicy = lockMemberPolicy;
            this.passwordPolicy = passwordPolicy;
            this.accountRepository = accountRepository;
        }

        public override string GenerateResetPasswordToken(string email, Guid clientId, string urlBack = "", int emailTemplateCode = 0)
        {
            if (email.IsNullOrWhiteSpace())
                throw new ArgumentException("E-mail não informado");

            ApplicationStore appSto;
            Account account;
            CustomerImport customerImport = null;
            string _tokenCode = string.Empty;

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    appSto = applicationStoreRepository.GetByClientId(clientId);
                    var accounts = accountRepository.Get(email, appSto, true);

                    accountService.lockedUpMemberPolicy = lockedUpMemberPolicy;
                    accountService.lockMemberPolicy = lockMemberPolicy;
                    accountService.passwordPolicy = passwordPolicy;

                    account = accountService.Authenticate(accounts, appSto, false);

                    if (account != null)
                    {
                        var resetPasswordTokenService = resetPasswordTokenFactory.GetResetPasswordTokenService(account);

                        resetPasswordTokenService.lockedUpMemberPolicy = lockedUpMemberPolicy;

                        var token = resetPasswordTokenService.GenerateResetPasswordToken(account, appSto, urlBack);

                        _tokenCode = token.Code.EncodeURIComponent();

                        svcEmail.SendPasswordRecoveryEmailAsync(account, appSto.Store, _tokenCode, token.UrlBack, emailTemplateCode);
                    }
                    else
                    {
                        customerImport = customerImportService.Get(email, appSto.Store.Code);

                        if (customerImport != null)
                        {
                            var resetPasswordTokenService = resetPasswordTokenFactory.GetResetPasswordTokenService(customerImport);

                            var token = resetPasswordTokenService.GenerateResetPasswordToken(customerImport, appSto, urlBack);

                            _tokenCode = token.Code.EncodeURIComponent();

                            customerImport.HandleCustomer();
                            svcEmail.SendPasswordRecoveryEmailAsync(customerImport, appSto.Store, _tokenCode, urlBack, emailTemplateCode);
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            if (!appSto.IsNull() && (!account.IsNull() || !customerImport.IsNull()))
            {
                using (var transaction = Connection.BeginTransaction())
                {
                    try
                    {
                        var code = !account.IsNull() ? account.Code : customerImport.AccountCode;

                        passwordLogRepository.Save(new PasswordLog(code, PasswordEventLog.RequestRecovery, appSto.Store.Code));

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }

            return _tokenCode;
        }
    }
}
