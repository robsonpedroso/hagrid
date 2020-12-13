namespace Hagrid.Infra.Contracts.Repository
{
    public interface IRepositoryAdd<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
    }
}