using Hagrid.Infra.Utils;
using log4net;
using System;
using System.Net;
using System.Reflection;

namespace Hagrid.Infra.Logging
{
    /// <summary>
    /// Log Info
    /// </summary>
    public abstract class MessageLogger
    {
        #region "  Properties  "

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Correlation id between request and response
        /// </summary>
        public Guid CorrelationId { get; protected set; }

        /// <summary>
        /// Ip address from client
        /// </summary>
        public string IpAddress { get; protected set; }

        /// <summary>
        /// Request line of message logger with headers and customs headers
        /// </summary>
        public string RequestLine { get; protected set; }

        /// <summary>
        /// Body of message logger
        /// </summary>
        public string Body { get; protected set; }

        /// <summary>
        /// Http Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public MessageLogger()
        {
            CorrelationId = Guid.NewGuid();
        }

        #endregion

        #region "  Request  "

        /// <summary>
        /// Log request sync on papertrailapp using log4net
        /// </summary>
        protected virtual void LogRequestSync()
        {
            try
            {
                ThreadContext.Properties["correlation_id"] = this.CorrelationId;
                ThreadContext.Properties["client_ip"] = this.IpAddress;

                var _message = "Request {0} {1}".ToFormat(this.RequestLine, this.Body);

                _log.Info(_message);
            }
            catch
            {
                //Not do anything
            }
        }

        #endregion

        #region "  Response  "

        /// <summary>
        /// Log reponse sync on papertrailapp using log4net
        /// </summary>
        protected virtual void LogResponseSync()
        {
            try
            {
                ThreadContext.Properties["correlation_id"] = this.CorrelationId;
                ThreadContext.Properties["client_ip"] = this.IpAddress;

                var _message = "Response {0} {1}".ToFormat(this.RequestLine, this.Body);

                switch ((HttpStatusCode)this.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.OK:
                        _log.Info(_message);
                        break;
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.BadRequest:
                        _log.Warn(_message);
                        break;
                    case HttpStatusCode.InternalServerError:
                        _log.Error(_message);
                        break;
                    default:
                        _log.Warn(_message);
                        break;
                }
            }
            catch
            {
                //Not do anything
            }
        }

        #endregion
    }
}