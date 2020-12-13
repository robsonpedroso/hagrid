using Hagrid.Core.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class BaseMetadataConfiguration<TEntityType> : EntityTypeConfiguration<TEntityType> where TEntityType : BaseMetadata
    {
        public BaseMetadataConfiguration()
        {
            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Metadata");
            Property(x => x.Value).HasColumnName("Value_Metadata");
            Property(x => x.SaveDate).HasColumnName("SaveDate_Metadata").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Metadata").IsRequired();

            HasRequired(x => x.Field).WithMany().Map(m => m.MapKey("Code_MetadataField"));
        }
    }
}