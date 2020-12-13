namespace Hagrid.Infra.Contracts
{
    /// <summary>
    /// Inteface to get all entities to put on the queue
    /// </summary>
    public interface IQueueable
    {
        /// <summary>
        /// Execute all entities to put on the queue
        /// </summary>
        void Enqueue();
    }
}