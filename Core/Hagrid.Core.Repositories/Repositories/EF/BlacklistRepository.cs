using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using System;
using System.Linq;
using System.Data.Entity;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class BlacklistRepository : BaseRepository<Blacklist, CustomerContext, Guid>, IBlacklistRepository
    {
        public BlacklistRepository(IConnection connection) : base(connection) { }

        public Blacklist GetUser(Guid accountCode, Guid? storeCode)
        {
            var result = from block in Collection
                         where block.AccountCode == accountCode
                         select block;

            result = result.Where(x => x.StoreCode == storeCode);

            result = result
                .Include(x => x.Store)
                .Include(x => x.Account);

            return result.FirstOrDefault();
        }

        public override Blacklist Get(Guid id)
        {
            var result = from block in Collection
                         where block.Code == id
                         select block;

            result = result
                .Include(x => x.Store)
                .Include(x => x.Account);

            return result.FirstOrDefault();
        }
    }
}
