using System.Data.Entity;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;

namespace Hagrid.Infra.Providers.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class EFBase<TEntity, TContext> : IEFRepository<EFContext>
        where TContext : EFContext
        where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        public IConnection Connection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbContext Context
        {
            get => ((EFConnection<TContext>)Connection).Context;
            set => ((EFConnection<TContext>)Connection).Context = value as TContext;
        }

        /// <summary>
        /// 
        /// </summary>
        protected DbSet<TEntity> Collection
        {
            get
            {
                if (!Context.IsNull())
                    return Context.Set<TEntity>();

                return default(DbSet<TEntity>);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EFBase()
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
