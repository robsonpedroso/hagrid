using Autofac;
using Hagrid.Core.Application;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;


namespace Hagrid.Core.IoC.Autofac
{
    public class ApplicationMapping
    {
        public static void Register(ContainerBuilder builder)
        {
            #region AccountApplication

            builder.Register(c => new AccountApplication(
                c.Resolve<IComponentContext>(),
                c.Resolve<IAccountService>(),
                c.Resolve<IAccountRepository>(),
                c.Resolve<IEmailSender>(),
                c.Resolve<ILockedUpMemberPolicy>(),
                c.Resolve<ILockMemberPolicy>(),
                c.Resolve<IPasswordPolicy>(),
                c.Resolve<IApplicationStoreRepository>(),
                c.Resolve<IResetPasswordTokenRepository>(),
                c.Resolve<IApplicationRepository>(),
                c.Resolve<IStoreRepository>(),
                c.Resolve<IAccountRoleRepository>(),
                c.Resolve<IRoleRepository>(),
                c.Resolve<IPermissionRepository>(),
                c.Resolve<IResourceRepository>(),
                c.Resolve<ICustomerImportService>(),
                c.ResolveNamed<IResetPasswordTokenService>("Account"),
                c.Resolve<IPasswordLogRepository>(),
                c.Resolve<IApplicationStoreService>(),
                c.Resolve<ICustomerRepository>(),
                c.Resolve<IAccountPermissionService>(),
                c.Resolve<IAccountInfraService>())).As<IAccountApplication>();
            #endregion

            builder.RegisterType<ApplicationStoreApplication>().As<IApplicationStoreApplication>();

            builder.RegisterType<ApplicationApplication>().As<IApplicationApplication>();

            builder.RegisterType<CustomerApplication>().As<ICustomerApplication>();

            builder.RegisterType<CustomerImportApplication>().As<ICustomerImportApplication>();
            
            builder.RegisterType<RefreshTokenApplication>().As<IRefreshTokenApplication>();

            builder.RegisterType<StoreApplication>().As<IStoreApplication>();

            builder.RegisterType<RequisitionApplication>().As<IRequisitionApplication>();

            builder.RegisterType<TransferTokenApplication>().As<ITransferTokenApplication>();

            builder.RegisterType<ResetPasswordTokenAccountApplication>().Named<IResetPasswordTokenApplication>("Account");

            builder.RegisterType<ResetPasswordTokenAccountApplication>().As<IAccountResetPasswordTokenApplication>();

            builder.Register(c => new ResetPasswordTokenCustomerImportApplication(
                c.Resolve<IComponentContext>(),
                c.ResolveNamed<IResetPasswordTokenService>("CustomerImport"),
                c.Resolve<ICustomerImportService>(),
                c.Resolve<IResetPasswordTokenRepository>(),
                c.Resolve<IApplicationRepository>(),
                c.Resolve<IApplicationStoreRepository>(),
                c.Resolve<IPasswordLogRepository>(),
                c.Resolve<IEmailSender>())).As<ICustomerImportResetPasswordTokenApplication>();

            builder.RegisterType<StoreCreditCardApplication>().As<IStoreCreditCardApplication>();

            builder.RegisterType<CreditCardApplication>().As<ICreditCardApplication>();

            builder.RegisterType<MetadataApplication>().As<IMetadataApplication>();

            builder.Register(c => new MetadataApplication(
                c.Resolve<IComponentContext>(),
                c.ResolveNamed<IMetadataService>(FieldType.Store.ToLower()),
                c.Resolve<IMetadataFieldRepository>())).Named<IMetadataApplication>(FieldType.Store.ToLower());

            builder.Register(c => new MetadataApplication(
                c.Resolve<IComponentContext>(),
                c.ResolveNamed<IMetadataService>(FieldType.Account.ToLower()),
                c.Resolve<IMetadataFieldRepository>())).Named<IMetadataApplication>(FieldType.Account.ToLower());

            builder.RegisterType<BlacklistApplication>().As<IBlacklistApplication>();

            builder.RegisterType<ResetSMSTokenApplication>().As<IResetSMSTokenApplication>();

            builder.RegisterType<ResourceApplication>().As<IResourceApplication>();

            builder.RegisterType<PermissionApplication>().As<IPermissionApplication>();

            builder.RegisterType<RoleApplication>().As<IRoleApplication>();

            builder.RegisterType<RestrictionApplication>().As<IRestrictionApplication>();
            builder.RegisterType<StoreAccountApplication>().As<IStoreAccountApplication>();
        }
    }
}
