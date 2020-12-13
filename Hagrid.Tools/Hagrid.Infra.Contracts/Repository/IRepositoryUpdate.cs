using System.Collections.Generic;
namespace Hagrid.Infra.Contracts.Repository
{
    public interface IRepositoryUpdate<TEntity> where TEntity : class
    {
        void Update(TEntity entity);
    }

    public interface IRepositoryUpdateObjs<TEntity> where TEntity : class
    {
        void Update(IEnumerable<TEntity> objs);
    }
}