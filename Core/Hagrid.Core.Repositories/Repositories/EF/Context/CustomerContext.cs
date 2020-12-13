using Autofac;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ModelValidation;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Infrastructure.Repositories.EF.Configuration;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Providers.EntityFramework;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hagrid.Core.Providers.EntityFramework.Context
{
    public class CustomerContext : EFContext, IDisposable
    {
        public DbSet<ApplicationStore> ApplicationsStores { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blacklist> BlackLists { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Requisition> FileImports { get; set; }
        public DbSet<StoreCreditCard> CreditCards { get; set; }
        public DbSet<CustomerImport> CustomerImports { get; set; }
        public DbSet<StoreMetadata> StoreMetadata { get; set; }
        public DbSet<AccountMetadata> AccountMetadata { get; set; }
        public DbSet<ResetSMSToken> ResetSMSTokens { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AccountApplicationStore> AccountsApplicationsStores { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }
        public DbSet<StoreAccount> StoreAccount { get; set; }

        private readonly IComponentContext context;

        public CustomerContext()
            : this(default) { }

        public CustomerContext(IComponentContext context)
            : base(ConfigurationManager.AppSettings["ConnectionStringName"].AsString())
            => this.context = context;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new RefreshTokenConfiguration());
            modelBuilder.Configurations.Add(new ResetPasswordTokenConfiguration());

            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new ApplicationStoreConfiguration());
            modelBuilder.Configurations.Add(new StoreConfiguration());
            modelBuilder.Configurations.Add(new StoreAddressConfiguration());

            modelBuilder.Configurations.Add(new TransferTokenConfiguration());

            modelBuilder.Configurations.Add(new CustomerImportConfiguration());
            modelBuilder.Configurations.Add(new PersonImportConfiguration());
            modelBuilder.Configurations.Add(new CompanyImportConfiguration());

            modelBuilder.Configurations.Add(new PasswordLogConfiguration());
            modelBuilder.Configurations.Add(new BlacklistConfiguration());
            modelBuilder.Configurations.Add(new StoreCreditCardConfiguration());

            modelBuilder.Configurations.Add(new RequisitionConfiguration());
            modelBuilder.Configurations.Add(new RequisitionErrorConfiguration());

            modelBuilder.Configurations.Add(new StoreMetadataConfiguration());
            modelBuilder.Configurations.Add(new AccountMetadataConfiguration());
            modelBuilder.Configurations.Add(new MetadataFieldConfiguration());

            modelBuilder.Configurations.Add(new ResetSMSTokenConfiguration());

            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AccountRoleConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new ResourceConfiguration());
            modelBuilder.Configurations.Add(new AccountApplicationStoreConfiguration());

            modelBuilder.Configurations.Add(new AuditLogsConfiguration());

            modelBuilder.Configurations.Add(new RestrictionConfiguration());

            modelBuilder.Configurations.Add(new StoreAccountConfiguration());

            modelBuilder.Entity<Person>();
            modelBuilder.Ignore<AccessPeriod>();
        }

        public override int SaveChanges()
        {
            try
            {
                AuditLog();

                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw ex;
            }
        }

        private void AuditLog()
        {
            try
            {
                var info = context.Resolve<IRequestInfoService>();
                if (info.IsNull())
                    return;

                var listAudit = new List<AuditLogs>();

                var modifiedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified || p.State == EntityState.Deleted).ToList();
                foreach (var change in modifiedEntities)
                {
                    var attsClass = change.Entity.GetType().GetCustomAttributes<AuditAttribute>();
                    if (!attsClass.IsNull() && attsClass.Count() > 0)
                    {
                        foreach (var propName in change.OriginalValues.PropertyNames)
                        {
                            var originalValue = change.OriginalValues[propName]?.ToString();
                            var currentValue = change.CurrentValues[propName]?.ToString();

                            if (originalValue != currentValue)
                            {
                                PropertyInfo propInfo = change.Entity.GetType().GetProperty(propName);
                                var atts = propInfo.GetCustomAttributes<AuditAttribute>();

                                if (!atts.IsNull() && atts.Count() > 0)
                                {
                                    var listAtts = atts as IEnumerable<AuditAttribute>;
                                    var audit = new Domain.Entities.AuditLogs(originalValue, currentValue);

                                    if (change.State == EntityState.Deleted)
                                        audit.AuditLogsType = Domain.Enums.AuditLogsType.Removed;
                                    else
                                        audit.AuditLogsType = listAtts.First().Type;

                                    var accCode = info.GetInfoRequest<Guid>("AccountCode");
                                    var appStoreCode = info.GetInfoRequest<Guid>("ApplicationStoreCode");
                                    var entityName = change.Entity.GetType().Name;
                                    var primaryKey = change.CurrentValues["Code"]?.ToString();

                                    audit.ReferenceCode = primaryKey.AsString();
                                    audit.ReferenceEntity = entityName;
                                    audit.AccountCode = accCode;
                                    audit.ApplicationStoreCode = appStoreCode;

                                    listAudit.Add(audit);
                                }
                            }
                        }
                    }
                }

                if (listAudit.Count > 0)
                {
                    var conn = context.Resolve<IConnection>();
                    var auditRep = context.Resolve<IAuditLogsRepository>();

                    conn.Open();
                    auditRep.Connection = conn;

                    using (var transaction = conn.BeginTransaction())
                    {
                        auditRep.Save(listAudit);
                        transaction.Commit();
                    }
                }
            }
            catch
            {
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}

