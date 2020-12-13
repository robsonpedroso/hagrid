using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IResetPasswordTokenService : IDomainService
    {
        ILockedUpMemberPolicy lockedUpMemberPolicy { get; set; }

        ResetPasswordToken GenerateResetPasswordToken(IResetPasswordTokenOwner owner, ApplicationStore applicationStore, string urlBack = "");

        void DeleteAllResetPasswordTokens(Guid ownerCode);
    }
}
