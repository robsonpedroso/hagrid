using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class ResetSMSTokenRepository : BaseRepository<ResetSMSToken, CustomerContext, string>, IResetSMSTokenRepository
    {
        public ResetSMSTokenRepository(IConnection connection) : base(connection) { }

        public List<ResetSMSToken> ListByOwnerCode(Guid ownerCode)
        {
            return Collection.Where(t => t.OwnerCode.Value.Equals(ownerCode)).ToList();
        }
    }
}
