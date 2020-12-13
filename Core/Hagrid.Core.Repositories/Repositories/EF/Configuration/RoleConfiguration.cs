using Hagrid.Core.Domain.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("THRole");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Role");

            HasRequired<Store>(r => r.Store)
                .WithMany(s => s.Roles)
                .HasForeignKey<Guid>(r => r.StoreCode);

            Property(x => x.StoreCode).HasColumnName("Code_Store").IsRequired();

            Property(x => x.Name).HasColumnName("Name_Role").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description_Role").IsRequired().HasMaxLength(300);
            Property(x => x.Status).HasColumnName("Status_Role").IsRequired();
            Property(x => x.Type).HasColumnName("Type_Role").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_Role").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Role").IsRequired();
        }
    }
}