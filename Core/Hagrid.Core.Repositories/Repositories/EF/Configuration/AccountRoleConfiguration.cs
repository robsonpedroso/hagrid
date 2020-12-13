using Hagrid.Core.Domain.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class AccountRoleConfiguration : EntityTypeConfiguration<AccountRole>
    {
        public AccountRoleConfiguration()
        {
            ToTable("THAccountRole");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_AccountRole");

            HasRequired<Account>(p => p.Account)
                .WithMany(r => r.AccountRoles)
                .HasForeignKey<Guid>(r => r.AccountCode);

            Property(x => x.AccountCode).HasColumnName("Code_Account").IsRequired();

            HasRequired<Role>(p => p.Role)
                .WithMany(r => r.AccountRoles)
                .HasForeignKey<Guid>(r => r.RoleCode);

            Property(x => x.RoleCode).HasColumnName("Code_Role").IsRequired();

            Property(x => x.Status).HasColumnName("Status_AccountRole").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_AccountRole").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_AccountRole").IsRequired();
        }
    }
}
