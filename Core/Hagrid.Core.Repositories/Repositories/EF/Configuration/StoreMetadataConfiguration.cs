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
    public class StoreMetadataConfiguration : BaseMetadataConfiguration<StoreMetadata>
    {
        public StoreMetadataConfiguration()
        {
            ToTable("THStoreMetadata");

            Property(x => x.StoreCode).HasColumnName("Code_Store").IsRequired();
        }
    }
}
