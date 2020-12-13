using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Hagrid.Infra.Utils.WebApi
{
    /// <summary>
    /// Provides details for authorization filter.
    /// </summary>
    public class JwtAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private static readonly string secretKey = ConfigurationManager.AppSettings["JwtTokenKey"];

        /// <summary>
        /// Roles
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppProperties { get; set; }

        /// <summary>
        /// Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context, which encapsulates information for using System.Web.Http.Filters.AuthorizationFilterAttribute.</param>
        protected virtual void OnAuthorizationSync(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (!authHeader.IsNull() && !authHeader.Scheme.IsNullOrWhiteSpace() && !authHeader.Parameter.IsNullOrWhiteSpace())
            {
                if (authHeader.Scheme.ToLower().Equals("bearer"))
                {
                    var token = authHeader.Parameter.TryDecodeJWT<JwtToken>(secretKey);

                    if (!token.IsNull() && !token.IsExpired())
                    {
                        if (IsValidRole(token) && IsValidProperties(token))
                        {
                            actionContext.ActionArguments.Add("identity", token);
                            return;
                        }
                    }
                }
            }

            actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context, which encapsulates information for using System.Web.Http.Filters.AuthorizationFilterAttribute.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            return Task.Run(() => OnAuthorizationSync(actionContext));
        }

        private bool IsValidRole(JwtToken token)
        {
            var isValid = true;

            if (!Roles.IsNullOrWhiteSpace())
            {
                var allowedRoles = Roles.Replace(", ", ",").Split(',');

                if (!allowedRoles.Contains(token.role))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool IsValidProperties(JwtToken token)
        {
            var isValid = true;

            if (!AppProperties.IsNullOrWhiteSpace())
            {
                var properties = AppProperties.Replace("; ", ",").Split(';');

                if (!properties.IsNull() && properties.Length > 0)
                {
                    properties.ForEach(item =>
                    {
                        var property = item.Replace("= ", "=").Replace(" =", "=").Split('=');

                        if (property.Length >= 2)
                        {
                            var name = "app:" + property[0];
                            var values = property[1].Replace(", ", ",").Split(',');

                            if (!token.clm.ContainsKey(name) || !values.Contains(token.clm[name]))
                            {
                                isValid = false;
                            }

                            if (property[0] == "member_type" && token.role == "Application")
                            {
                                isValid = true;
                            }
                        }
                    });
                }
            }

            return isValid;
        }
    }
}