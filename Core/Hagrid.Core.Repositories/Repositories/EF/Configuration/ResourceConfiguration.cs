using Hagrid.Core.Domain.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class ResourceConfiguration : EntityTypeConfiguration<Resource>
    {
        public ResourceConfiguration()
        {
            ToTable("THResource");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Resource");

            HasRequired<Application>(r => r.Application)
                .WithMany(app => app.Resources)
                .HasForeignKey<Guid>(r => r.ApplicationCode);

            Property(x => x.ApplicationCode).HasColumnName("Code_Application").IsRequired();

            Property(x => x.InternalCode).HasColumnName("InternalCode_Resource").IsRequired().HasMaxLength(50).IsOptional();
            Property(x => x.Name).HasColumnName("Name_Resource").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description_Resource").HasMaxLength(300);
            Property(x => x.Operations).HasColumnName("Operations_Resource").IsRequired();
            Property(x => x.Type).HasColumnName("Type_Resource").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_Resource").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Resource").IsRequired();
        }
    }
}
