using Autofac;
using Hagrid.Core.Domain.Contracts.Services;
using DO = Hagrid.Core.Domain.Services.Requisitions;

namespace Hagrid.Core.IoC.Autofac.Requisitions
{
    public class CustomerImportDBMapping
    {
        private static readonly string instanceName = "importinternalaccounts";

        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<DO.CustomerImportDBService>().Named<IRequisitionProcessingService>(instanceName);
        }
    }
}
