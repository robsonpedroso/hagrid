using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace Hagrid.Infra.Utils.WebApi
{
    /// <summary>
    /// Defines properties and methods to result for Wrapper API controller.
    /// </summary>
    public class WrapperApiController : ApiController
    {
        /// <summary>
        /// Set correlation id - The correlation id is simply, it's a GUID (globally unique identifier) that's automatically generated for every request that the server receives.
        /// </summary>
        protected Action<string> SetCorrelationId;

        /// <summary>
        /// Initializes the System.Web.Http.ApiController instance with the specified controllerContext.
        /// </summary>
        /// <param name="controllerContext">The System.Web.Http.Controllers.HttpControllerContext object that is used for the initialization.</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            if (!this.SetCorrelationId.IsNull())
            {
                var correlationId = ActionContext.Request.Headers.GetValues("CorrelationId").FirstOrDefault();

                this.SetCorrelationId(correlationId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public new OkNegotiatedContentResult<APIResult<T>> Ok<T>(T content) where T : class
        {
            return new OkNegotiatedContentResult<APIResult<T>>(new APIResult<T>(content), this);
        }

        /// <summary>
        /// Creates an System.Web.Http.Results.OkResult (200 OK).
        /// </summary>
        /// <returns>An System.Web.Http.Results.OkResult.</returns>
        public virtual ResponseMessageResult Ok<T>(T content, params string[] ignoreFields) where T : class
        {
            var ok = Request.CreateResponse(HttpStatusCode.OK);

            ok.Content = new APIResult<T>(content).GetContent();

            ok.Headers.Add("X-Ignore-Fields", ignoreFields?.Select(i => $"content.{i}"));

            return new ResponseMessageResult(ok);
        }

        /// <summary>
        /// Creates an System.Web.Http.Results.OkResult (200 OK).
        /// </summary>
        /// <returns>An System.Web.Http.Results.OkResult.</returns>
        public new IHttpActionResult Ok()
        {
            var ok = Request.CreateResponse(HttpStatusCode.OK);

            ok.Content = new APIResult().GetContent();

            return new ResponseMessageResult(ok);
        }

        /// <summary>
        /// Creates an System.Web.Http.Results.ExceptionResult (500 Internal Server Error) with the specified exception.
        /// </summary>
        /// <param name="ex">The exception to include in the error.</param>
        /// <returns>An System.Web.Http.Results.ExceptionResult with the specified exception.</returns>
        public new IHttpActionResult InternalServerError(Exception ex)
        {
            var error = Request.CreateResponse(HttpStatusCode.InternalServerError);

            Request.Properties.Add("Exception", ex);

            var owinContext = Request.GetOwinContext();

            if (!owinContext.IsNull())
            {
                owinContext.Set("Exception", ex);
                ex.Data.Add("CorrelationId", owinContext.Request.Headers["CorrelationId"]);
            }

            error.Content = new APIResult<Exception>(ex).GetContent();

            return new ResponseMessageResult(error);
        }

        /// <summary>
        /// Creates a System.Web.Http.Results.BadRequestResult.
        /// </summary>
        /// <param name="arg">An ArgumentException with the specified exception.</param>
        /// <returns>A System.Web.Http.Results.BadRequestResult.</returns>
        public IHttpActionResult BadRequest(ArgumentException arg)
        {
            var bad = Request.CreateResponse(HttpStatusCode.BadRequest);

            Request.Properties.Add("Exception", arg);

            var owinContext = Request.GetOwinContext();

            if (!owinContext.IsNull())
            {
                owinContext.Set("Exception", arg);
                owinContext.Set("IsHandledError", true);
            }

            bad.Content = new APIResult<ArgumentException>(arg).GetContent();

            return new ResponseMessageResult(bad);
        }

        /// <summary>
        /// Creates a System.Web.Http.Results.ForbiddenResult.
        /// </summary>
        /// <returns>A System.Web.Http.Results.ForbiddenResult.</returns>
        public IHttpActionResult Forbidden()
        {
            var forbidden = Request.CreateResponse(HttpStatusCode.Forbidden);

            forbidden.Content = new APIResult<Exception>(new Exception("Esta requisição não é permitida")).GetContent();

            return new ResponseMessageResult(forbidden);
        }
    }

    /// <summary>
    /// Result base API
    /// </summary>
    public class APIResult
    {
        /// <summary>
        /// Status result OK or ERROR
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// List of message 
        /// </summary>
        public IEnumerable<object> messages { get; set; }

        /// <summary>
        /// Constructor APIResult 
        /// </summary>
        public APIResult()
        {
            this.status = "OK";

            this.messages = this.messages.Append(new
            {
                type = "SUCCESS",
                text = "Operação realizada com sucesso."
            });
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamWriter class for the specified stream by using UTF-8 encoding and the default buffer size.
        /// </summary>
        /// <returns></returns>
        public HttpContent GetContent()
        {
            var content = new PushStreamContent((stream, httpContent, transportContext) =>
            {
                using (var tw = new StreamWriter(stream))
                {
                    var ser = new JsonSerializer()
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                        MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                        Formatting = Newtonsoft.Json.Formatting.None,
                        FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
                        FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double,
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                        DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                        ContractResolver = new ProxyContractResolver(),
                        PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None,
                    };

                    ser.Converters.Add(new StringEnumConverter());

                    ser.Serialize(tw, this);
                }
            });

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }
    }

    /// <summary>
    /// Result base API typed
    /// </summary>
    public class APIResult<T> : APIResult where T : class
    {
        /// <summary>
        /// Cotent to response in json
        /// </summary>
        public T content { get; set; }

        /// <summary>
        /// Constructor APIResult typed result
        /// </summary>
        /// <param name="content"></param>
        public APIResult(T content)
            : base()
        {
            if (content is Exception)
            {
                var showAllExceptions = false;

                var ex = (content as Exception);

                this.status = "ERROR";

                if (showAllExceptions)
                {
                    this.messages = GetMessagesRecursivelly(ex);
                }
                else
                {
                    var message = string.Empty;

                    if (!ex.Data.IsNull() && ex.Data.Contains("CorrelationId"))
                        message = string.Format("Falha ao processar a requisição. Por favor entre em contato com o responsável fornecendo o código de erro: {0}.", ex.Data["CorrelationId"].ToString());
                    else
                        message = ex.GetBaseException().Message;

                    this.messages = new object[]
                    {
                        new
                        {
                            type = "ERROR",
                            text = message,
                            trace = ex.GetType() != typeof(ArgumentException) && !ex.InnerException.IsNull() ? ex.GetBaseException().StackTrace : null
                        }
                    };
                }
            }
            else
            {
                this.messages = new object[0];
                this.content = content;
            }
        }

        private List<object> GetMessagesRecursivelly(Exception ex, List<object> list = null)
        {
            if (list.IsNull())
                list = new List<object>();

            if (!ex.IsNull())
            {
                list.Add(new
                {
                    type = "ERROR",
                    text = ex.Message,
                    trace = ex.GetType() != typeof(ArgumentException) ? ex.StackTrace : null
                });

                GetMessagesRecursivelly(ex.InnerException, list);
            }

            return list;
        }
    }
}