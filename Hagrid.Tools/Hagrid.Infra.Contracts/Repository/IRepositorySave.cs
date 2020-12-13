
namespace Hagrid.Infra.Contracts.Repository
{
    public interface IRepositorySave<TEntity> where TEntity : class
    {
        TEntity Save(TEntity entity);
    }
}
