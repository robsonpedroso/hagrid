using log4net;
using log4net.Core;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Hagrid.Infra.Logging.Slack
{
    /// <summary>
    /// Simple, asynchronous Slack log4net messager.
    /// </summary>    
    public class SlackMessager : INotifier
    {
        /// <summary>
        /// The correlation ID is simply, it's a GUID (globally unique identifier) that's automatically generated for every request that the server receives.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected ILog _logger;

        /// <summary>
        /// 
        /// </summary>
        protected Log4Slack.SlackAppender _appender;

        /// <summary>
        /// Constructor
        /// </summary>
        public SlackMessager()
            : this("SlackMessager")
        {
        }

        /// <summary>
        /// Constructor with name logger
        /// </summary>
        public SlackMessager(string name)
        {
            _logger = LogManager.GetLogger(Assembly.GetCallingAssembly(), name);
            log4net.Config.XmlConfigurator.Configure(_logger.Logger.Repository, new FileInfo("log4net.config"));
            _appender = new Log4Slack.SlackAppender();
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Info level.
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
        public void Info(object message)
        {
            Send(message, level: Level.Info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception)
        {
            Send(message, exception, Level.Info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            Send(message, level: Level.Warn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception)
        {
            Send(message, exception, Level.Warn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            Send(exception.Message, exception, Level.Error);
        }

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception object to log.</param>
        /// <remarks>
        /// See the Error(object) form for more detailed information.
        /// </remarks>        
        public void Error(object message, Exception exception)
        {
            Send(message, exception, Level.Error);
        }

        /// <summary>
        /// Send all logs
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="level"></param>
        protected async void Send(object message, Exception exception = null, Level level = null)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (this.CorrelationId.IsNullOrWhiteSpace())
                        this.CorrelationId = Guid.NewGuid().ToString();

                    ThreadContext.Properties["correlation_id"] = this.CorrelationId; ;

                    switch (level.Name)
                    {
                        case "INFO":
                            _logger.Info(message, exception);
                            break;

                        case "WARN":
                            _logger.Warn(message, exception);
                            break;

                        case "ERROR":
                            _logger.Error(message, exception);
                            break;

                        default:
                            _logger.Info(message, exception);
                            break;
                    }
                }
                catch
                {
                    //do not stop
                }
            });
        }
    }

    /// <summary>
    /// Simple, asynchronous Slack log4net messager.
    /// </summary>    
    /// <typeparam name="T">Type of class useed</typeparam>
    public class SlackMessager<T> : SlackMessager, INotifier
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SlackMessager() : this(typeof(T).Name) { }

        /// <summary>
        /// Constructor with name logger
        /// </summary>
        public SlackMessager(string name) : base(name) { }

        /// <summary>
        /// Generate correlationID
        /// </summary>
        public void GenerateCorrelationID()
        {
            CorrelationId = Guid.NewGuid().ToString();
        }
    }
}