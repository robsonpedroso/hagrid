
namespace Hagrid.Infra.Contracts
{
    /// <summary>
    /// IWorker
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Start server for process the commands in queues
        /// </summary>
        /// <param name="workerCount">Proccesses in parallel</param>
        /// <param name="queueNames">List of queue names</param>
        void Run(int workerCount = 0, params string[] queueNames);

        /// <summary>
        /// Stop server and dispose
        /// </summary>
        void Stop();
    }
}
