using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Hagrid.Infra.Utils.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class HashAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private static readonly string hashString = ConfigurationManager.AppSettings["JwtTokenKey"];

        private void OnAuthorizationSync(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null && !string.IsNullOrWhiteSpace(authHeader.Scheme) && !string.IsNullOrWhiteSpace(authHeader.Parameter))
            {
                if (authHeader.Scheme.ToLower().Equals("hash") && authHeader.Parameter.Equals(hashString))
                    return;
            }

            actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            return Task.Run(() => OnAuthorizationSync(actionContext));
        }
    }
}
