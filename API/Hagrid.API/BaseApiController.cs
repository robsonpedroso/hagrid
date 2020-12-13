using Autofac;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Linq;
using System.Web.Http;
using Hagrid.Core.IoC.Autofac;
using System.Web.Http.Dependencies;
using Autofac.Integration.WebApi;

namespace Hagrid.API
{
    public class BaseApiController : WrapperApiController
    {
        private readonly IRequestInfoService info;

        public BaseApiController(IRequestInfoService info)
            => this.info = info;

        private JwtToken identity;

        internal JwtToken Identity
        {
            get
            {
                if (identity.IsNull())
                    identity = ActionContext.ActionArguments["identity"] as JwtToken;

                return identity;
            }
        }

        internal Guid ClientId
        {
            get
            {
                switch (Identity.role)
                {
                    case "Application":
                        return Identity.name.AsGuid();
                    case "Member":
                    case "ChangePassword":
                    case "ResetPassword":
                        return identity.clm.FirstOrDefault(c => c.Key.Equals("system")).Value.AsGuid();
                    default:
                        return Guid.Empty;
                }
            }
        }

        internal bool IsMainAdmin => Identity.clm.FirstOrDefault(c => c.Key.Equals("app:member_type")).Value.AsString().ToLower() == "mainadmin";

        internal string AccountEmail => Identity.name;

        internal Guid AccountCode => Identity.clm.FirstOrDefault(c => c.Key.Equals("sid")).Value.AsGuid();

        internal Guid StoreCode => Identity.clm.FirstOrDefault(c => c.Key.Equals("app:store_code")).Value.AsGuid();

        internal ILifetimeScope CurrentContainer => ((AutofacWebApiDependencyResolver)ControllerContext?.Configuration?.DependencyResolver).Container;

        internal T GetApplication<T>() where T : class
        {
            var app = CurrentContainer.Resolve<T>();

            return app;
        }

        internal T GetApplication<T>(string instanceName) where T : class
        {
            var app = CurrentContainer.ResolveNamed<T>(instanceName);

            return app;
        }

        internal void SetRequestInfo()
        {
            info.SetInfoRequest("AccountCode", AccountCode);
            info.SetInfoRequest("ApplicationStoreCode", ClientId);
        }

        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping() => Ok();
    }
}