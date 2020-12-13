using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Services
{
    public class ResetPasswordTokenCustomerImportService : ResetPasswordTokenService
    {
        private IResetPasswordTokenRepository resetPasswordTokenRepository;

        public ResetPasswordTokenCustomerImportService(IResetPasswordTokenRepository resetPasswordTokenRepository): base (resetPasswordTokenRepository)
        {
            this.resetPasswordTokenRepository = resetPasswordTokenRepository;
        }

        public override ResetPasswordToken GenerateResetPasswordToken(IResetPasswordTokenOwner owner, ApplicationStore applicationStore, string urlBack = "")
        {
            var customer = owner as CustomerImport;

            ResetPasswordToken token = null;
            var tokens = resetPasswordTokenRepository.ListByOwnerCode(customer.AccountCode);

            if (!tokens.IsNull())
            {
                token = tokens.Where(t => t.ExpiresUtc > DateTime.UtcNow.AddMinutes(15) && t.ApplicationStoreCode == applicationStore.Code).FirstOrDefault();

                if (!token.IsNull() && (DateTime.UtcNow - token.GeneratedUtc).TotalSeconds > Config.SecondsToRegeneratePasswordRecovery)
                {
                    token = null;
                }
            }

            if (token.IsNull())
            {
                base.DeleteAllResetPasswordTokens(customer.AccountCode);

                token = new ResetPasswordToken(customer.AccountCode, applicationStore.Code, urlBack);
                resetPasswordTokenRepository.Add(token);
            }

            return token;
        }


        #region "  IDomainService  "

        public override List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { resetPasswordTokenRepository };
        }

        #endregion
    }
}
