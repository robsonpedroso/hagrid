using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class ResetPasswordTokenRepository : BaseRepository<ResetPasswordToken, CustomerContext, string>, IResetPasswordTokenRepository
    {
        public ResetPasswordTokenRepository(IConnection connection) : base(connection) { }

        public List<ResetPasswordToken> ListByOwnerCode(Guid ownerCode)
        {
            return Collection.Where(t => t.OwnerCode.Value.Equals(ownerCode)).ToList();
        }
    }
}
