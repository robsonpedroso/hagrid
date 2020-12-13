using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class AccountMetadataRepository : BaseRepository<AccountMetadata, CustomerContext, Guid>, IMetadataRepository<AccountMetadata>
    {
        public AccountMetadataRepository(IConnection connection) : base(connection) { }

        public AccountMetadata Get(AccountMetadata metadata)
        {
            return Collection
                .Where(c => c.Field.JsonId == metadata.Field.JsonId && c.AccountCode == metadata.AccountCode)
                .FirstOrDefault();
        }

        public bool HasValueByJsonId(BaseMetadata metadata)
        {
            return Collection.Any(c => c.Field.JsonId == metadata.Field.JsonId);
        }
    }
}