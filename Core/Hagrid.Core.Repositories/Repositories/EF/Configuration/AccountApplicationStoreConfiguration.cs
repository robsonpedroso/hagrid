using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class AccountApplicationStoreConfiguration : EntityTypeConfiguration<AccountApplicationStore>
    {
        public AccountApplicationStoreConfiguration()
        {
            ToTable("THAccountApplicationStore");

            HasKey(a => a.Code);
            Property(x => x.Code).HasColumnName("Code_AccountApplicationStore");

            HasRequired(a => a.Account)
                .WithMany(app => app.AccountApplicationStoreCollection)
                .HasForeignKey(a => a.AccountCode)
                .WillCascadeOnDelete(false);
            Property(x => x.AccountCode).HasColumnName("Code_Account");

            HasRequired(a => a.ApplicationStore)
                .WithMany(app => app.AccountApplicationStoreCollection)
                .HasForeignKey(a => a.ApplicationStoreCode)
                .WillCascadeOnDelete(false);
            Property(x => x.ApplicationStoreCode).HasColumnName("Code_ApplicationStore");

            Property(x => x.SaveDate).HasColumnName("SaveDate_AccountApplicationStore").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_AccountApplicationStore").IsRequired();
        }
    }
}
