using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class StoreMetadataRepository : BaseRepository<StoreMetadata, CustomerContext, Guid>, IMetadataRepository<StoreMetadata>
    {
        public StoreMetadataRepository(IConnection connection) : base(connection) { }

        public StoreMetadata Get(StoreMetadata metadata)
        {
            return Collection
                .FirstOrDefault(c => c.Field.JsonId == metadata.Field.JsonId && c.StoreCode == metadata.StoreCode);
        }

        public bool HasValueByJsonId(BaseMetadata metadata)
        {
            return Collection.Any(c => c.Field.JsonId == metadata.Field.JsonId);
        }
    }
}