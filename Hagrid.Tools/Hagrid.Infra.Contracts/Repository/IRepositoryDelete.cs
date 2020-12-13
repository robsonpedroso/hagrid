using System;
using System.Collections.Generic;

namespace Hagrid.Infra.Contracts.Repository
{
    public interface IRepositoryDelete<TEntity> where TEntity : class
    {
        void Delete(TEntity entity);
    }

    public interface IRepositoryDeleteByCode<TKey>
    {
        void Delete(TKey code);
    }

    public interface IRepositoryDeleteMany<TEntity> where TEntity : class
    {
        void DeleteMany(IEnumerable<TEntity> items);
    }
}