using System;
using System.Threading.Tasks;

namespace Hagrid.Infra.Contracts.Repository
{
    /// <summary>
    /// Repository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Connection Object
        /// </summary>
        IConnection Connection { get; set; }
    }

    /// <summary>
    /// Repository Methods
    /// </summary>
    /// <typeparam name="TEntity">Entity Mapped</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        #region "  Methods  "

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <param name="entity">Object to be inserted</param>
        /// <returns>Saved entity</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity">Object to be deleted</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete Entity by Id
        /// </summary>
        /// <param name="id">UniqueIdentifier Id</param>
        void Delete(Guid id);

        /// <summary>
        /// Get Entity by Id
        /// </summary>
        /// <param name="id">UniqueIdentifier Id</param>
        /// <returns>Object correspondent to Id</returns>
        TEntity Get(Guid id);

        /// <summary>
        /// Save or Update Entity
        /// </summary>
        /// <param name="entity">Object. If not exists, save, else update</param>
        /// <returns>Saved or Updated Object</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Object to be updated</param>
        void Update(TEntity entity);

        #endregion

        #region "  Async Methods  "

        /// <summary>
        /// Insert Entity Async
        /// </summary>
        /// <param name="entity">Object to be inserted</param>
        /// <returns>Saved entity</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Delete Entity Async
        /// </summary>
        /// <param name="entity">Object to be deleted</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete Entity by Id Async
        /// </summary>
        /// <param name="id">UniqueIdentifier Id</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Get Entity by Id Async
        /// </summary>
        /// <param name="id">UniqueIdentifier Id</param>
        /// <returns>Object correspondent to Id</returns>
        Task<TEntity> GetAsync(Guid id);

        /// <summary>
        /// Save or Update Entity Async
        /// </summary>
        /// <param name="entity">Object. If not exists, save, else update</param>
        /// <returns>Saved or Updated Object</returns>
        Task<TEntity> SaveAsync(TEntity entity);

        /// <summary>
        /// Update Entity Async
        /// </summary>
        /// <param name="entity">Object to be updated</param>
        Task UpdateAsync(TEntity entity);

        #endregion
    }
}
