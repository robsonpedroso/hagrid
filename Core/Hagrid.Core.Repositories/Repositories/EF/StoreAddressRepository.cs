using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class StoreAddressRepository : BaseRepository<StoreAddress, CustomerContext, Guid>, IStoreAddressRepository
    {
        public StoreAddressRepository(IConnection connection) : base(connection) { }

        public IEnumerable<StoreAddress> GetByStore(Guid storeCode, bool activeRecordsOnly = true)
        {
            return Context
                .Set<StoreAddress>()
                .Where(s => s.StoreCode == storeCode && s.Status == activeRecordsOnly)
                .ToList();
        }
    }
}
