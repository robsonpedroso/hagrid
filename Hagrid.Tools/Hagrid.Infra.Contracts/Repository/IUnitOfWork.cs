using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hagrid.Infra.Contracts.Repository
{
    /// <summary>
    /// The repository and unit of work patterns are intended to create an abstraction
    /// layer between the data access layer and the business logic layer of an application
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Is the connection still open?
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Create a database connection and open a session on it
        /// </summary>
        /// <param name="setCommitFlushMode">Determines at which points Hibernate automatically flushes the session.</param>
        /// <param name="interceptor">Interceptor DataBase for audit</param>
        /// <returns>A session.</returns>
        IUnitOfWork Open(bool setCommitFlushMode = false, IDBInterceptor interceptor = null);

        /// <summary>
        ///  Get a new stateless session.
        /// </summary>
        /// <param name="setCommitFlushMode"></param>
        /// <returns>A stateless session</returns>
        IUnitOfWork OpenStateless(bool setCommitFlushMode = false);

        /// <summary>
        /// Begin a unit of work and return the associated ITransaction object.
        /// </summary>
        /// <param name="isolation">Isolation level for the new transaction</param>
        /// <param name="setCommitFlushMode">Determines at which points Hibernate automatically flushes the session.
        /// <param name="isAsync">Is async for the new transaction</param>
        /// For a readonly session, it is reasonable to set the flush mode to FlushMode.Never
        /// at the start of the session (in order to achieve some extra performance).
        /// </param>
        /// <returns>A transaction instance</returns>
        /// <remarks>
        /// If a new underlying transaction is required, begin the transaction. Otherwise
        /// continue the new work in the context of the existing underlying transaction.
        /// The class of the returned INHUnitOfWork object is determined by the property transaction.
        /// </remarks>
        IUnitOfWorkTransaction BeginTransaction(string isolation = null, bool setCommitFlushMode = false, bool isAsync = false);

        /// <summary>
        /// Get session current connection
        /// </summary>
        /// <returns></returns>
        object GetSession();

        /// <summary>
        /// Get stateless session current connection
        /// </summary>
        /// <returns></returns>
        object GetStatelessSession();
    }
}
