using Autofac;
using Hagrid.Core.Application;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Infrastructure.Repositories.EF;
using Hagrid.Core.Infrastructure.Services.Accounts;
using Hagrid.Core.Infrastructure.Services.Customer;
using Hagrid.Core.Infrastructure.Services.Email;
using Hagrid.Core.Infrastructure.Services.SMS;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Logging.Slack;

namespace Hagrid.Core.IoC.Autofac
{
    public class InfraMapping
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.Register(c => new CustomerContext(c));
            builder.RegisterType<CustomerConnection>().As<IConnection>().InstancePerLifetimeScope();

            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<AccountPermissionRepository>().As<IAccountPermissionRepository>();
            builder.RegisterType<ApplicationRepository>().As<IApplicationRepository>();
            builder.RegisterType<ApplicationStoreRepository>().As<IApplicationStoreRepository>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<CustomerImportRepository>().As<ICustomerImportRepository>();
            builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>();
            builder.RegisterType<ResetPasswordTokenRepository>().As<IResetPasswordTokenRepository>();
            builder.RegisterType<StoreRepository>().As<IStoreRepository>();
            builder.RegisterType<StoreAddressRepository>().As<IStoreAddressRepository>();
            builder.RegisterType<TransferTokenRepository>().As<ITransferTokenRepository>();
            builder.RegisterType<PasswordLogRepository>().As<IPasswordLogRepository>();
            builder.RegisterType<StoreCreditCardRepository>().As<IStoreCreditCardRepository>();
            builder.RegisterType<RequisitionRepository>().As<IRequisitionRepository>();
            builder.RegisterType<BlacklistRepository>().As<IBlacklistRepository>();
            builder.RegisterType<ResourceRepository>().As<IResourceRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<AccountRoleRepository>().As<IAccountRoleRepository>();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>();
            builder.RegisterType<ResetSMSTokenRepository>().As<IResetSMSTokenRepository>();
            builder.RegisterType<EmailSender>().As<IEmailSender>().UsingConstructor(typeof(INotifier));
            builder.RegisterType<MetadataFieldRepository>().As<IMetadataFieldRepository>();
            builder.RegisterType<AccountMetadataRepository>().As<IMetadataRepository<AccountMetadata>>();
            builder.RegisterType<StoreMetadataRepository>().As<IMetadataRepository<StoreMetadata>>();
            builder.RegisterType<AccountApplicationStoreRepository>().As<IAccountApplicationStoreRepository>();

            builder.RegisterType<AccountInfraService>().As<IAccountInfraService>();
            builder.RegisterType<CustomerImportFileInfraService>().As<ICustomerImportFileInfraService>();
            builder.RegisterType<CustomerImportDBInfraService>().As<ICustomerImportDBInfraService>();
            builder.RegisterType<SlackMessager<AccountBaseApplication>>().As<INotifier>();

            builder.RegisterType<RestrictionRepository>().As<IRestrictionRepository>();
            builder.RegisterType<StoreAccountRepository>().As<IStoreAccountRepository>();

            builder.RegisterType<SMSInfraService>().As<ISmsInfraService>();
        }
    }
}
