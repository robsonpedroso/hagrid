using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hagrid.Infra.Contracts.Repository
{
    /// <summary>
    ///  Allows the application to define units of work, while maintaining 
    ///  abstraction from the underlying transaction implementation    
    /// </summary>
    /// <remarks>
    ///  A transaction is associated with a ISession and is usually instantiated by a call to beginTransaction(). 
    ///  A single session might span multiple transactions since the notion of a session (a conversation between the application and the datastore) 
    ///  is of coarser granularity than the notion of a transaction. However, it is intended  that there be 
    ///  at most one uncommitted ITransaction associated with a particular session at a time. Implementors are not intended to be threadsafe.
    /// </remarks>
    public interface IUnitOfWorkTransaction : IDisposable
    {
        /// <summary>
        /// Is the ts async transaction?
        /// </summary>
        bool IsAsync { get; }

        /// <summary>
        /// Is the connection still open?
        /// </summary>
        bool IsConnectionOpen { get; }

        /// <summary>
        /// Force the underlying transaction to roll back.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Force the underlying transaction to roll back.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
        /// <returns>A cancellation token that can be used to cancel the work</returns>
        Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Flush the associated ISession and end the unit of work.
        /// </summary>
        /// <remarks>
        /// This method will commit the underlying transaction if and only if the transaction was initiated by this object.
        /// </remarks>
        void Commit();

        /// <summary>
        /// Flush the associated ISession and end the unit of work.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
        /// <remarks>
        /// This method will commit the underlying transaction if and only if the transaction was initiated by this object.
        /// </remarks>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///  Sets the batch size of the session
        /// </summary>
        /// <param name="size">batchSize</param>
        void SetBatchSize(int size);
    }
}
