using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class RestrictionConfiguration : EntityTypeConfiguration<Restriction>
    {
        public RestrictionConfiguration()
        {
            ToTable("THRestriction");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Restriction");

            HasRequired(p => p.Role)
                .WithMany(r => r.Restrictions)
                .HasForeignKey(r => r.RoleCode);

            Property(x => x.RoleCode).HasColumnName("Code_Role").IsRequired();

            Property(x => x.SaveDate).HasColumnName("SaveDate_Restriction");
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Restriction");

            Property(x => x.ObjectSerialized).HasColumnName("Object_Requisition").IsRequired();

            Map<IpAddressRestriction>(m => m.Requires("Type_Restriction").HasValue((int)RestrictionType.IpAddress));
            Map<PeriodRestriction>(m => m.Requires("Type_Restriction").HasValue((int)RestrictionType.Period));
        }
    }
}
