using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owin;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hagrid.Infra.Logging.WebApi
{
    /// <summary>
    /// Log info for web api
    /// </summary>
    internal class MessageLoggerWebApi : MessageLogger
    {
        private string[] contentAllowedLog = new string[] {
            "application/json",
            "text/json",
            "application/javascript",
            "text/javascript",
            "application/x-www-form-urlencoded"
        };

        public MessageLoggerWebApi(IOwinRequest request)
            : base()
        {
            IpAddress = request.RemoteIpAddress;
        }

        #region "  Request  "

        public void LogRequestSync(IOwinRequest request)
        {
            RequestLine = string.Format("{0} {1} {2} {3}", request.Method, request.Path, request.Headers["Authorization"], request.Headers.GetCustomHeader()).Trim();

            if (request.Headers["CorrelationId"] == null)
                request.Headers.Add("CorrelationId", new[] { this.CorrelationId.ToString() });

            var allowed = contentAllowedLog.Any(i => request.Headers["Content-Type"].AsString("").Contains(i));

            if (allowed)
            {
                Body = ReadBodyStreamAsString(request.Body);
            }
            else
            {
                Body = "Body-Ignored";
            }

            LogRequestSync();
        }

        public async Task LogRequestAsync(IOwinRequest request)
        {
            await Task.Run(() => { LogRequestSync(request); });
        }

        #endregion

        #region "  Response  "

        public void LogResponseSync(IOwinResponse response)
        {
            var request = response.Context.Request;

            RequestLine = string.Format("{0} {1}", request.Method, request.Path).Trim();

            StatusCode = (HttpStatusCode)response.StatusCode;

            var allowed = contentAllowedLog.Any(i => response.Headers["Content-Type"].AsString("").Contains(i));

            if (allowed)
            {
                Body = HandleResponseMessage(response);

                if (Body.ValidateJSON())
                {
                    var ignores = response.Headers.FirstOrDefault(x => x.Key == "X-Ignore-Fields");

                    if (!ignores.IsNull() && ignores.Value?.Count() > 0)
                    {
                        var body = JToken.Parse(Body).RemoveFields(ignores.Value);

                        Body = body.ToString(Formatting.None);
                    }
                }
            }
            else
            {
                Body = "Body-Ignored";
            }

            LogResponseSync();
        }

        public async Task LogResponseAsync(IOwinResponse response)
        {
            await Task.Run(() => { LogResponseSync(response); });
        }

        #endregion

        #region "  Private  "

        private string ReadBodyStreamAsString(Stream bodyStream)
        {
            if (bodyStream.IsNull() || bodyStream.Length <= 0)
                return string.Empty;

            if (bodyStream.CanSeek)
                bodyStream.Seek(0, SeekOrigin.Begin);

            var reqBytes = new byte[bodyStream.Length];
            bodyStream.Read(reqBytes, 0, reqBytes.Length);
            bodyStream.Seek(0, SeekOrigin.Begin);

            return Encoding.UTF8.GetString(reqBytes).TrimJson();
        }

        private string HandleResponseMessage(IOwinResponse response)
        {
            if (response.IsSuccessStatusCode())
            {
                return ReadBodyStreamAsString(response.Body);
            }

            var requestEx = response.Get<Exception>("Exception");

            if (!requestEx.IsNull())
            {
                if (!response.Get<bool>("IsHandledError"))
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return response.Get<Exception>("Exception").TrimMessage();
            }

            return HandleHttpError(response);
        }

        private string HandleHttpError(IOwinResponse response)
        {
            var bodyString = ReadBodyStreamAsString(response.Body);

            if (!bodyString.IsNullOrWhiteSpace())
            {
                try
                {
                    JObject error = JObject.Parse(bodyString);

                    var exceptionMessage = error["ExceptionMessage"];
                    var stackTrace = error["StackTrace"];

                    if (!exceptionMessage.IsNull() && !stackTrace.IsNull())
                    {
                        var message = new
                        {
                            message = exceptionMessage.ToString(),
                            traces = stackTrace.ToString()
                        };

                        return message.ToJsonString();
                    }
                }
                catch
                {
                    return response.ReasonPhrase;
                }
            }

            return response.ReasonPhrase;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MessageLoggerWebApiExtensions
    {
        /// <summary>
        /// Use http message logger to log pappertrail
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IAppBuilder UseHttpMessageLogger(this IAppBuilder app)
        {
            return app.Use<OwinMessageLogger>();
        }

        /// <summary>
        /// Use http message logger to log pappertrail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IAppBuilder UseHttpMessageLogger<T>(this IAppBuilder app) where T : class
        {
            return app.Use<OwinMessageLogger>();
        }

        /// <summary>
        /// Get Client IpAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            const string HttpContext = "MS_HttpContext";
            const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
            const string OwinContext = "MS_OwinContext";

            try
            {
                // Web-hosting. Needs reference to System.Web.dll
                if (request.Properties.ContainsKey(HttpContext))
                {
                    dynamic ctx = request.Properties[HttpContext];
                    if (ctx != null)
                    {
                        return ctx.Request.UserHostAddress;
                    }
                }

                // Self-hosting. Needs reference to System.ServiceModel.dll. 
                if (request.Properties.ContainsKey(RemoteEndpointMessage))
                {
                    dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                    if (remoteEndpoint != null)
                    {
                        return remoteEndpoint.Address;
                    }
                }

                // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
                if (request.Properties.ContainsKey(OwinContext))
                {
                    dynamic owinContext = request.Properties[OwinContext];
                    if (owinContext != null)
                    {
                        return owinContext.Request.RemoteIpAddress;
                    }
                }
            }
            catch (Exception)
            {
                // Always return all zeroes for any failure (my calling code expects it)
                return "0.0.0.0";
            }

            return null;
        }

        /// <summary>
        /// Get custom header starts with "X-"
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string GetCustomHeader(this IHeaderDictionary header)
        {
            try
            {
                var startWith = header.Where(h => h.Key.StartsWith("X-"));

                var extraHeaders = string.Join(" ", startWith.Select(h => string.Format("{0}={1}", h.Key, string.Join(" ", h.Value).Trim()))); //get extras headers

                return extraHeaders.Trim();
            }
            catch
            {
                // Always return all zeroes for any failure (my calling code expects it)
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool IsSuccessStatusCode(this IOwinResponse response)
        {
            return response.StatusCode >= 200 && response.StatusCode <= 299;
        }

        /// <summary>
        /// Validate JSON
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ValidateJSON(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        /// <summary>
        /// Remove Fields from JSON
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static JToken RemoveFields(this JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            if (container.IsNull() || fields.IsNull()) return token;

            var removeList = new List<JToken>();

            foreach (var el in container.Children())
            {
                if (el is JProperty p && fields.Contains(Regex.Replace(p.Path, @"\[\d*\]\.", ".")))
                    removeList.Add(el);

                el.RemoveFields(fields);
            }

            foreach (var el in removeList)
                el.Remove();

            return token;
        }
    }
}