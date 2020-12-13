using Microsoft.Owin;
using Autofac.Integration.Owin;
using Autofac;

namespace Hagrid.API.Providers
{
    public static class AutofacOwinExtensions
    {
        public static T Resolve<T>(this IOwinContext context)
            => context.GetAutofacLifetimeScope().Resolve<T>();

        public static T ResolveNamed<T>(this IOwinContext context, string serviceName)
            => context.GetAutofacLifetimeScope().ResolveNamed<T>(serviceName);
    }
}