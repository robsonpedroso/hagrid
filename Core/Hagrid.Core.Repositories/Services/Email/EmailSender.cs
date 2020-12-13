using Hagrid.Core.Domain;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Providers.Rmail;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private RmailProvider provider;
        private INotifier notify;

        public EmailSender(INotifier notify)
        {
            provider = new RmailProvider();
            this.notify = notify;
        }

        public void SendPasswordRecoveryEmailAsync(Account account, Store store, string tokenCode, string urlBack = "", int emailTemplateCode = 0)
        {
            Task.Run(() => SendPasswordRecoveryEmail(account, store, tokenCode, urlBack, emailTemplateCode));
        }

        public void SendPasswordRecoveryEmailAsync(CustomerImport customer, Store store, string tokenCode, string urlBack = "", int emailTemplateCode = 0)
        {
            Task.Run(() => SendPasswordRecoveryEmail(customer, store, tokenCode, urlBack, emailTemplateCode));
        }

        public void SendEmailUserCreatedByAccountAdminAsync(Account account, string tokenCode, string urlBack = "")
        {
            Task.Run(() => SendEmailUserCreatedByAccountAdmin(account, tokenCode, urlBack));
        }

        private void SendEmailUserCreatedByAccountAdmin(Account account, string tokenCode, string urlBack = "")
        {
            var accountsURL = Config.AccountsSiteURL;

            if (!accountsURL.IsNullOrWhiteSpace())
            {
                var input = new 
                {
                    CamapaignSubtypeCode = account.CodeEmailTemplate.Value,
                    To = new []
                    {
                        new  { Email = account.Email }
                    },
                    Data = new []
                    {
                        new { HashTag = "#email_account#", Value = account.Email },
                        new 
                        {
                            HashTag = "#passwordrecovery_url#",
                            Value = urlBack.IsNullOrWhiteSpace() ?
                                string.Format("{0}/{1}/{2}", accountsURL, Properties.RecoveryPasswordLocation, tokenCode.EncodeURIComponent()) :
                                string.Format("{0}/{1}/{2}?ub={3}", accountsURL, Properties.RecoveryPasswordLocation, tokenCode.EncodeURIComponent(), urlBack)
                        }
                    }
                };

                try
                {
                    var output = provider.SendEmail(input);

                    if (!output.IsNull())
                        notify.Warn("Fail send e-mail - SendEmailUserCreatedByAccountAdmin - Account: Message: {0}".ToFormat(output));
                }
                catch (Exception ex)
                {
                    notify.Error("Fail send e-mail - SendEmailUserCreatedByAccountAdmin - Account", ex);
                }

            }
        }

        private void SendPasswordRecoveryEmail(Account account, Store store, string tokenCode, string urlBack = "", int emailTemplateCode = 0)
        {
            var accountsURL = Config.AccountsSiteURL;

            if (!string.IsNullOrWhiteSpace(accountsURL))
            {
                var input = new 
                {
                    CamapaignSubtypeCode = emailTemplateCode > 0 ? emailTemplateCode : (int)EnumCampaignSubtype.System_ForgotPasswordShopping,
                    To = new []
                    {
                        new { Email = account.Email }
                    },
                    Data = new []
                    {
                        new { HashTag = "#name_store#", Value = store.Name },
                        new { HashTag = "#logo_store#", Value = store.GetLogoURL() },
                        new
                        {
                            HashTag = "#passwordrecovery_url#",
                            Value = urlBack.IsNullOrWhiteSpace() ?
                                string.Format("{0}/{1}/{2}", accountsURL, Properties.RecoveryPasswordLocation, tokenCode.EncodeURIComponent()) :
                                string.Format("{0}/{1}/{2}?ub={3}", accountsURL, Properties.RecoveryPasswordLocation, tokenCode.EncodeURIComponent(), urlBack)
                        }
                    }
                };


                try
                {
                    var output = provider.SendEmail(input);

                    if (!output.IsNull())
                        notify.Warn("Fail send e-mail - SendPasswordRecoveryEmail - Account: Message: {0}".ToFormat(output));
                }
                catch (Exception ex)
                {
                    notify.Error("Fail send e-mail - SendPasswordRecoveryEmail - Account", ex);
                }

            }
        }

        private void SendPasswordRecoveryEmail(CustomerImport customer, Store store, string tokenCode, string urlBack = "", int emailTemplateCode = 0)
        {
            var accountsURL = Config.AccountsSiteURL;

            if (!string.IsNullOrWhiteSpace(accountsURL))
            {
                var input = new                 {
                    CamapaignSubtypeCode = emailTemplateCode > 0 ? emailTemplateCode : (int)EnumCampaignSubtype.System_CustomerImportCreatePassword,
                    To = new []
                    {
                        new { Email = customer.Email }
                    }
                };

                try
                {
                    var output = provider.SendEmail(input);

                    if (!output.IsNull())
                        notify.Warn("Fail send e-mail - SendPasswordRecoveryEmail - Customer: Message: {0}".ToFormat(output));
                }
                catch (Exception ex)
                {
                    notify.Error("Fail send e-mail - SendPasswordRecoveryEmail - Customer", ex);
                }
            }
        }
    }

    enum EnumCampaignSubtype
    {
        System_ForgotPasswordShopping = 9,
        System_CustomerImportCreatePassword = 83
    }
}
