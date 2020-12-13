using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Application
{
    public class ResetPasswordTokenCustomerImportApplication : ResetPasswordTokenApplication, ICustomerImportResetPasswordTokenApplication
    {
        private readonly IResetPasswordTokenService resetPasswordTokenService;
        private readonly ICustomerImportService customerImportService;
        private readonly IApplicationRepository applicationRepository;
        private readonly IApplicationStoreRepository applicationStoreRepository;
        private readonly IPasswordLogRepository passwordLogRepository;
        private readonly IEmailSender svcEmail;

        public ResetPasswordTokenCustomerImportApplication(
            IComponentContext context,
            IResetPasswordTokenService resetPasswordTokenService,
            ICustomerImportService customerImportService,
            IResetPasswordTokenRepository resetPasswordTokenRepository,
            IApplicationRepository applicationRepository,
            IApplicationStoreRepository applicationStoreRepository,
            IPasswordLogRepository passwordLogRepository,
            IEmailSender svcEmail)
            : base(context, resetPasswordTokenRepository, applicationStoreRepository)
        {
            this.resetPasswordTokenService = resetPasswordTokenService;
            this.customerImportService = customerImportService;
            this.applicationRepository = applicationRepository;
            this.applicationStoreRepository = applicationStoreRepository;
            this.passwordLogRepository = passwordLogRepository;
            this.svcEmail = svcEmail;
        }

        public override string GenerateResetPasswordToken(string email, Guid storeCode, string urlBack = "", int emailTemplateCode = 0)
        {
            ApplicationStore appSto;
            CustomerImport customerImport;
            string _tokenCode = string.Empty;

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var application = applicationRepository.Get("EC-Loja", true);

                    appSto = applicationStoreRepository.Get(application.Code.Value, storeCode, true);

                    customerImport = customerImportService.Get(email, storeCode);

                    if (customerImport != null)
                    {
                        var token = resetPasswordTokenService.GenerateResetPasswordToken(customerImport, appSto, urlBack);

                        _tokenCode = token.Code.EncodeURIComponent();

                        customerImport.HandleCustomer();
                        svcEmail.SendPasswordRecoveryEmailAsync(customerImport, appSto.Store, _tokenCode, token.UrlBack, emailTemplateCode);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            if (!appSto.IsNull() && !customerImport.IsNull())
            {
                using (var transaction = Connection.BeginTransaction())
                {
                    try
                    {
                        passwordLogRepository.Save(new PasswordLog(customerImport.AccountCode, PasswordEventLog.ResquetRecoryCustomerImport, appSto.Store.Code));
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
