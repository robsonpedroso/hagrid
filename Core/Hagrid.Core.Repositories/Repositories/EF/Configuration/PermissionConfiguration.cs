using Hagrid.Core.Domain.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            ToTable("THPermission");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Permission");

            HasRequired<Role>(p => p.Role)
                            .WithMany(r => r.Permissions)
                            .HasForeignKey<Guid>(r => r.RoleCode);

            Property(x => x.RoleCode).HasColumnName("Code_Role").IsRequired();

            HasRequired<Resource>(p => p.Resource)
                .WithMany(r => r.Permissions)
                .HasForeignKey<Guid>(r => r.ResourceCode);

            Property(x => x.ResourceCode).HasColumnName("Code_Resource").IsRequired();
            Property(x => x.Operations).HasColumnName("Operations_Permission").IsRequired();
            Property(x => x.Status).HasColumnName("Status_Permission").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_Permission").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Permission").IsRequired();
        }
    }
}
