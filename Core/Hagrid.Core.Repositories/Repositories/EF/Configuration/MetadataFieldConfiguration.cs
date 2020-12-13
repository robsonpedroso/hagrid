using Hagrid.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class MetadataFieldConfiguration : EntityTypeConfiguration<MetadataField>
    {
        public MetadataFieldConfiguration()
        {
            ToTable("THMetadataField");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_MetadataField");
            Property(x => x.JsonId).HasColumnName("JsonId_MetadataField").HasMaxLength(50).IsRequired();
            Property(x => x.Name).HasColumnName("Name_MetadataField").HasMaxLength(256).IsRequired();
            Property(x => x.Type).HasColumnName("Type_MetadataField").IsRequired();
            Property(x => x.Format).HasColumnName("Format_MetadataField").IsRequired();
            Property(x => x.ValidatorSerialized).HasColumnName("Validator_MetadataField");
            Property(x => x.SaveDate).HasColumnName("SaveDate_MetadataField").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_MetadataField").IsRequired();

            Property(t => t.Name)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_MetadataField_Name", 1)));

            Property(t => t.JsonId)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_MetadataField_JsonId", 1)));
        }
    }
}