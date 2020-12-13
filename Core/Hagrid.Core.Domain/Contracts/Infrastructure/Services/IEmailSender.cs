using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface IEmailSender
    {   
        void SendPasswordRecoveryEmailAsync(Account account, Store store, string tokenCode, string urlBack = "", int emailTemplateCode = 0);

        void SendPasswordRecoveryEmailAsync(CustomerImport customer, Store store, string tokenCode, string urlBack = "", int emailTemplateCode = 0);

        void SendEmailUserCreatedByAccountAdminAsync(Account account, string tokenCode, string urlBack = "");
    }
}