using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class StoreCreditCardRepository : BaseRepository<StoreCreditCard, CustomerContext, Guid>, IStoreCreditCardRepository
    {
        public StoreCreditCardRepository(IConnection connection) : base(connection) { }
    }
}
