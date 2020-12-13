using System;
using System.Threading.Tasks;

namespace Hagrid.Infra.Contracts
{
    /// <summary>
    /// Interface to make pissible send notification contract
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// The correlation ID is simply, it's a GUID (globally unique identifier) that's automatically generated for every request that the server receives.
        /// </summary>
        string CorrelationId { get; set; }

        /// <summary>
        /// Logs a message object with the Info level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <remarks>
        /// This method first checks if this logger is INFO enabled by comparing the
        /// level of this logger with the log4net.Core.Level.Info level. If this logger
        /// is INFO enabled, then it converts the message object (passed as parameter)
        /// to a string by invoking the appropriate log4net.ObjectRenderer.IObjectRenderer.
        /// It then proceeds to call all the registered appenders in this logger and
        /// also higher in the hierarchy depending on the value of the additivity flag.
        /// WARNING Note that passing an System.Exception to this method will print the
        /// name of the System.Exception but no stack trace. To print a stack trace use
        /// the Info(object,Exception) form instead.
        /// </remarks>
        void Info(object message);

        /// <summary>
        /// Logs a message object with the Info level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception object to log.</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Logs a message object with the Warn level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(object message);

        /// <summary>
        /// Logs a message object with the Warn level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception object to log.</param>
        void Warn(object message, Exception exception);
        
        /// <summary>
        /// Logs a message object with the Error level.        
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception object to log.</param>
        void Error(Exception exception);

        /// <summary>
        /// Logs a message object with the Error level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception object to log.</param>
        void Error(object message, Exception exception);
    }
}