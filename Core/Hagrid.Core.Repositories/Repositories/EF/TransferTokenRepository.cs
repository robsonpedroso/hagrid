using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class TransferTokenRepository : BaseRepository<TransferToken, CustomerContext, string>, ITransferTokenRepository
    {
        public TransferTokenRepository(IConnection connection) : base(connection) { }

        public TransferToken GetByOwner(Guid ownerCode)
        {
            return Collection.Where(t => t.OwnerCode == ownerCode).FirstOrDefault();
        }
    }
}
