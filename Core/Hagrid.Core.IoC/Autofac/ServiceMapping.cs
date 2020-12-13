using Autofac;
using Hagrid.Core.Domain.Contracts.Factories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Factories;
using Hagrid.Core.Domain.Services;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Providers.IO;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.IoC.Autofac
{
    public static class ServiceMapping
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<RequestInfoService>().As<IRequestInfoService>();

            builder.RegisterType<AccountService>().As<IAccountService>();

            builder.RegisterType<AccountPermissionService>().As<IAccountPermissionService>();

            builder.RegisterType<ApplicationStoreService>().As<IApplicationStoreService>();

            builder.RegisterType<CustomerService>().As<ICustomerService>();

            builder.RegisterType<CustomerImportService>().As<ICustomerImportService>();

            builder.RegisterType<StoreService>().As<IStoreService>();

            builder.RegisterType<RefreshTokenService>().As<IRefreshTokenService>();

            builder.RegisterType<RequisitionService>().As<IRequisitionService>();

            builder.RegisterType<ResetPasswordTokenAccountService>().Named<IResetPasswordTokenService>("Account");

            builder.RegisterType<ResetPasswordTokenCustomerImportService>().Named<IResetPasswordTokenService>("CustomerImport");

            builder.RegisterType<IORepository>().As<IIORepository>();

            builder.RegisterType<ResetPasswordTokenFactory>().As<IResetPasswordTokenFactory>();

            builder.RegisterType<CreditCardService>().As<ICreditCardService>();

            builder.RegisterType<MetadataService<BaseMetadata>>().As<IMetadataService>();

            builder.Register(c => new MetadataService<StoreMetadata>(
                c.Resolve<IMetadataFieldRepository>(),
                c.Resolve<IMetadataRepository<StoreMetadata>>(),
                c.Resolve<IComponentContext>())).Named<IMetadataService>(FieldType.Store.ToLower());

            builder.Register(c => new MetadataService<AccountMetadata>(
                c.Resolve<IMetadataFieldRepository>(),
                c.Resolve<IMetadataRepository<AccountMetadata>>(),
                c.Resolve<IComponentContext>())).Named<IMetadataService>(FieldType.Account.ToLower());

            builder.RegisterType<BlacklistService>().As<IBlacklistService>();

            builder.RegisterType<ResetSMSTokenService>().As<IResetSMSTokenService>();

            builder.RegisterType<AccountApplicationStoreService>().As<IAccountApplicationStoreService>();

            builder.RegisterType<RoleService>().As<IRoleService>();
        }
    }
}
