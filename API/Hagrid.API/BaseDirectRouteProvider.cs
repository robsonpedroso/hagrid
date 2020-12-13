using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Hagrid.API
{
    public class BaseDirectRouteProvider : DefaultDirectRouteProvider
    {
        // inherit route attributes decorated on base class controller's actions
        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
            => actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
    }
}