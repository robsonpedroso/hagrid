using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Providers.EntityFramework;
using System;

namespace Hagrid.Core.Infrastructure.Repositories
{
    public class BaseRepository<TEntity, TContext, TKey> : EFBaseRepository<TEntity, TContext, TKey>, IDisposable
        where TEntity : class, IEntity<TKey>
        where TContext : EFContext
    {
        public BaseRepository(IConnection connection)
            : base()
            => Connection = connection;

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
