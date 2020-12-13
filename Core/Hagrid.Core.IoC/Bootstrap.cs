using System.Reflection;

namespace Hagrid.Core.IoC
{
    public static class Bootstrap
    {
        public static void Go(params Assembly[] assemblies)
        {
            Autofac.Current.Setup(assemblies);
        }
    }
}
