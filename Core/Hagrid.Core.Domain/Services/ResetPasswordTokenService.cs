using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Services
{
    public abstract class ResetPasswordTokenService : IResetPasswordTokenService
    {
        private IResetPasswordTokenRepository resetPasswordTokenRepository;

        public ILockedUpMemberPolicy lockedUpMemberPolicy { get; set; }

        public ResetPasswordTokenService(IResetPasswordTokenRepository resetPasswordTokenRepository)
        {
            this.resetPasswordTokenRepository = resetPasswordTokenRepository;
        }

        public abstract ResetPasswordToken GenerateResetPasswordToken(IResetPasswordTokenOwner owner, ApplicationStore applicationStore, string urlBack = "");

        public void DeleteAllResetPasswordTokens(Guid ownerCode)
        {
            var tokens = resetPasswordTokenRepository.ListByOwnerCode(ownerCode);

            if (tokens != null)
                resetPasswordTokenRepository.DeleteMany(tokens);
        }

        #region "  IDomainService  "

        public abstract List<IRepository> GetRepositories();

        #endregion
    }
}
