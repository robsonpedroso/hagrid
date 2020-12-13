using System;
using System.Collections.Generic;

namespace Hagrid.Infra.Contracts.Repository
{
    public interface IRepositoryGet<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        TEntity Get(TKey code);
    }

    public interface IRepositoryGet<TEntity> where TEntity : IEntity
    {
        TEntity Get(Guid code);
    }

    public interface IRepositoryList<TEntity> where TEntity : class
    {
        List<TEntity> List();
    }
}