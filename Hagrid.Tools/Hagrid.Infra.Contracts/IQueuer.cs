using System;
using System.Linq.Expressions;

namespace Hagrid.Infra.Contracts
{
    /// <summary>
    /// IQueuer
    /// </summary>
    public interface IQueuer
    {
        /// <summary>
        /// Include command in queue to be run only once
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="queueName">Queue name</param>
        /// <returns></returns>
        string Enqueue(Expression<Action> methodCall, string queueName = "default");

        /// <summary>
        /// Include command in queue to be run only once
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="queueName">Queue name</param>
        /// <returns></returns>
        string Enqueue<T>(Expression<Action<T>> methodCall, string queueName = "default");
        
        /// <summary>
        /// Schedule or compile to run in a certain schedule
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="delay">TimeSpan the delay for execution comamand</param>
        /// <param name="queueName">Queue name</param>
        /// <returns></returns>
        string Enqueue(Expression<Action> methodCall, TimeSpan delay, string queueName = "default");
        
        /// <summary>
        /// Schedule or compile to run in a certain schedule
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="delay">TimeSpan the delay for execution comamand</param>
        /// <param name="queueName">Queue name</param>
        /// <returns></returns>
        string Enqueue<T>(Expression<Action<T>> methodCall, TimeSpan delay, string queueName = "default");

        /// <summary>
        /// Schedule or compile to run in a certain schedule
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="enqueueAt">DateTime for execution command</param>
        /// <param name="queueName">Queue name</param>
        /// <returns></returns>
        string Enqueue(Expression<Action> methodCall, DateTime enqueueAt, string queueName = "default");
        
        /// <summary>
        /// Schedule or compile to run in a certain schedule
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="enqueueAt">DateTime for execution command</param>
        /// <param name="queueName">Queue name</param>
        /// <returns></returns>
        string Enqueue<T>(Expression<Action<T>> methodCall, DateTime enqueueAt, string queueName = "default");
       
        /// <summary>
        /// Create a recurrence for command
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="cron">Rule in the cron type for executing the command</param>
        /// <param name="queueName"></param>
        void Recurring(Expression<Action> methodCall, string cron, string queueName = "default");

        /// <summary>
        /// Create a recurrence for command
        /// </summary>
        /// <param name="methodCall">Action that contains the method to be executed</param>
        /// <param name="cron">Rule in the cron type for executing the command</param>
        /// <param name="queueName"></param>
        void Recurring<T>(Expression<Action<T>> methodCall, string cron, string queueName = "default");

        /// <summary>
        /// Delete recurrency from queue by recurringJobId
        /// </summary>
        /// <param name="jobId"></param>
        void DeleteRecurrency(string jobId);

        /// <summary>
        /// Delete recurrency from queue by jobId
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        bool Delete(string jobId);
    }
}
