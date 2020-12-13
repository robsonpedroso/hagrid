using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Policies;
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
    public class ResetPasswordTokenAccountService : ResetPasswordTokenService
    {
        private IResetPasswordTokenRepository resetPasswordTokenRepository;

        public ResetPasswordTokenAccountService(IResetPasswordTokenRepository resetPasswordTokenRepository) : base (resetPasswordTokenRepository)
        {
            this.resetPasswordTokenRepository = resetPasswordTokenRepository;
        }

        public override ResetPasswordToken GenerateResetPasswordToken(IResetPasswordTokenOwner owner, ApplicationStore applicationStore, string urlBack = "")
        {
            var account = owner as Account;

            ResetPasswordToken token = null;
            var tokens = resetPasswordTokenRepository.ListByOwnerCode(account.Code);

            if (!tokens.IsNull())
            {
                token = tokens.Where(t => t.ExpiresUtc > DateTime.UtcNow.AddMinutes(60) && t.ApplicationStoreCode == applicationStore.Code).FirstOrDefault();

                if (!token.IsNull())
                    return token;
            }

            if (token.IsNull())
            {
                base.DeleteAllResetPasswordTokens(account.Code);

                token = new ResetPasswordToken(account.Code, applicationStore.Code, urlBack);
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
