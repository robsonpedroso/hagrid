using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class StoreConfiguration : EntityTypeConfiguration<Store>
    {
        public StoreConfiguration()
        {
            ToTable("THStore");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Store");
            Property(x => x.Name).HasColumnName("Name_Store").IsRequired().HasMaxLength(50);
            Property(x => x.Cnpj).HasColumnName("Cnpj_Store").HasMaxLength(14);
            Property(x => x.IsMain).HasColumnName("IsMain_Store").IsRequired();
            Property(x => x.Status).HasColumnName("Status_Store").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_Store").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Store").IsRequired();

            HasMany<StoreAddress>(s => s.Addresses)
                .WithRequired(s => s.Store)
                .HasForeignKey(s => s.StoreCode);

            HasMany<StoreMetadata>(s => s.Metadata)
                .WithRequired(s => s.Store)
                .HasForeignKey(s => s.StoreCode);

            Property(t => t.Name)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Store_NameStore", 1)));

            Property(t => t.Status)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Store_Status", 1)));            
        }
    }
}
