using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Hagrid.API.Providers;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.IoC;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Reflection;
using System.Web.Http;
using Hagrid.Infra.Logging.WebApi;

[assembly: OwinStartup(typeof(Hagrid.API.Startup))]

namespace Hagrid.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Bootstrap.Go(Assembly.GetExecutingAssembly());

            app.UseCors(CorsOptions.AllowAll);
            app.UseHttpMessageLogger<Startup>();

            ConfigureAutofac(app, out HttpConfiguration config);
            ConfigureOAuth(app);
            ConfigureWebApi(app, config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = Config.Environment != Core.Domain.Enums.EnvironmentType.Production ? true: false,
                AccessTokenExpireTimeSpan = new TimeSpan(0, Convert.ToInt32(Config.TokenExpirationTimeInMinutes), 0),
                TokenEndpointPath = new PathString("/token"),
                AuthorizeEndpointPath = new PathString("/authorize"),
                Provider = new AuthenticationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider(),
                AuthorizationCodeProvider = new AuthorizationCodeProvider(),
                AccessTokenFormat = new JwtTokenFormat()
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public void ConfigureWebApi(IAppBuilder app, HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(new BaseDirectRouteProvider());

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.None,
                FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
                FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ContractResolver = new ProxyContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None
            };

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            app.UseWebApi(config);
        }

        public void ConfigureAutofac(IAppBuilder app, out HttpConfiguration config)
        {
            config = new HttpConfiguration();
            var container = Core.IoC.Autofac.Current.Container.BeginLifetimeScope();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
        }
    }
}