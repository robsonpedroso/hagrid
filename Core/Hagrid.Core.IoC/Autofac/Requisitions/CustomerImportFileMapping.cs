using Autofac;
using Hagrid.Core.Domain.Contracts.Services;
using DO = Hagrid.Core.Domain.Services.Requisitions;

namespace Hagrid.Core.IoC.Autofac.Requisitions
{
    public class CustomerImportFileMapping
    {
        private static readonly string instanceName = "importexternalaccounts";

        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<DO.CustomerImportFileService>().Named<IRequisitionProcessingService>(instanceName);
        }
    }
}
