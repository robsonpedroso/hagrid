using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IResetPasswordTokenRepository : IRepository, IRepositoryGet<ResetPasswordToken, String>, IRepositoryAdd<ResetPasswordToken>, 
        IRepositoryDelete<ResetPasswordToken>, IRepositoryDeleteByCode<String>, IRepositoryDeleteMany<ResetPasswordToken>
    {
        List<ResetPasswordToken> ListByOwnerCode(Guid ownerCode);
    }
}
