using Autofac;
using Autofac.Integration.WebApi;
using Hagrid.Infra.Utils;
using System.Reflection;

namespace Hagrid.Core.IoC.Autofac
{
    public class Current
    {
        private volatile static object _lock = new object();
        private static IContainer container;
        public static IContainer Container
        {
            get
            {
                Setup();
                return container;
            }
        }

        public static void Setup(params Assembly[] assemblies)
        {
            lock (_lock)
            {
                if (container.IsNull())
                {
                    var builder = new ContainerBuilder();
                    PolicyMapping.Register(builder);
                    InfraMapping.Register(builder);
                    ServiceMapping.Register(builder);
                    ApplicationMapping.Register(builder);

                    Requisitions.CustomerImportFileMapping.Register(builder);
                    Requisitions.CustomerImportDBMapping.Register(builder);

                    if (assemblies != null && assemblies.Length > 0)
                    {
                        builder.RegisterAssemblyTypes(assemblies).PropertiesAutowired();
                        builder.RegisterApiControllers(assemblies);
                    }

                    container = builder.Build();
                }
            }
        }
    }
}
