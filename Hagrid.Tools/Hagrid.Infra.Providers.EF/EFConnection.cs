using Hagrid.Infra.Contracts.Repository;
using System;
using System.Data;
using Hagrid.Infra.Utils;

namespace Hagrid.Infra.Providers.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class EFConnection<TContext> : IConnection where TContext : EFContext
    {
        private bool disposed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EFContext Context { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ITransaction Transaction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EFConnection()
        {
            Context = Activator.CreateInstance<TContext>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public EFConnection(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOpen
        {
            get { return Context.Database.Exists() && Context.Database.Connection.State.Equals(ConnectionState.Open); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setCommitFlushMode"></param>
        public void Open(bool setCommitFlushMode = false)
        {
            if (disposed)
                return;

            this.Context.Database.Connection.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setCommitFlushMode"></param>
        public void OpenStateless(bool setCommitFlushMode = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            this.Context.Database.Connection.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolation"></param>
        /// <param name="setCommitFlushMode"></param>
        /// <returns></returns>
        public ITransaction BeginTransaction(string isolation = null, bool setCommitFlushMode = false)
        {
            IsolationLevel? _isolation = null;

            if (!string.IsNullOrWhiteSpace(isolation))
                _isolation = (IsolationLevel)Enum.Parse(typeof(IsolationLevel), isolation, true);

            this.Transaction = new EFTransaction(this.Context, _isolation);

            return this.Transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (Transaction != null)
                        Transaction.Dispose();

                    if (!this.Context.IsNull() && this.Context.Database.Exists() && this.Context.Database.Connection.State.Equals(ConnectionState.Open))
                        this.Context.Dispose();
                }
            }

            this.disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDispose()
        {
            return disposed;
        }
    }
}
