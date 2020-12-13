using System.Collections.Generic;
using System.Linq;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Infra.Providers.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EFBaseRepository<TEntity, TContext, TKey>
        : EFBase<TEntity, TContext>, 
        IRepositoryAdd<TEntity>, 
        IRepositorySave<TEntity>, 
        IRepositoryUpdate<TEntity>, 
        IRepositoryUpdateObjs<TEntity>, 
        IRepositoryDelete<TEntity>, 
        IRepositoryDeleteByCode<TKey>, 
        IRepositoryDeleteMany<TEntity>, 
        IRepositoryGet<TEntity, TKey>, 
        IRepositoryList<TEntity>
        where TContext : EFContext
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity obj)
        {
            return Collection.Add(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual TEntity Save(TEntity obj)
        {
            var original = Collection.Find(obj.Code);

            if (original == null)
            {
                return Collection.Add(obj);
            }
            else
            {
                Context.Entry(original).CurrentValues.SetValues(obj);
                Context.Entry(original).State = System.Data.Entity.EntityState.Modified;

                return Collection.Find(obj.Code);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Update(TEntity obj)
        {
            Collection.Attach(obj);
            Context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        public virtual void Update(IEnumerable<TEntity> objs)
        {
            objs.ToList().ForEach(obj =>
            {
                Collection.Attach(obj);
                Context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Delete(TEntity obj)
        {
            Collection.Attach(obj);
            Collection.Remove(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(TKey id)
        {
            var instance = Collection.Find(id);
            Collection.Remove(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public virtual void DeleteMany(IEnumerable<TEntity> items)
        {
            foreach (var obj in items)
                Collection.Attach(obj);

            Collection.RemoveRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Get(TKey id)
        {
            return Collection.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> List()
        {
            return Collection.ToList();
        }
    }
}
