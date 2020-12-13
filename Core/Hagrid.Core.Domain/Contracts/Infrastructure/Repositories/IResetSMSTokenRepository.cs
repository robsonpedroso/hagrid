using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IResetSMSTokenRepository : IRepository, IRepositoryGet<ResetSMSToken, String>, IRepositoryAdd<ResetSMSToken>,
        IRepositoryDelete<ResetSMSToken>, IRepositoryDeleteByCode<String>, IRepositoryDeleteMany<ResetSMSToken>, IRepositoryUpdate<ResetSMSToken>
    {
        List<ResetSMSToken> ListByOwnerCode(Guid ownerCode);
    }
}
