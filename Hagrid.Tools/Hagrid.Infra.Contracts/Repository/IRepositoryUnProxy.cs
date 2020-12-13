using System.Collections.Generic;

namespace Hagrid.Infra.Contracts.Repository
{
    public interface IRepositoryUnProxy<TEntity> where TEntity : class
    {
        TEntity UnProxy(TEntity proxied);
        IEnumerable<TEntity> UnProxy(IEnumerable<TEntity> proxied);
    }
}
