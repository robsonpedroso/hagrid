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
    public class StoreAddressConfiguration : EntityTypeConfiguration<StoreAddress>
    {
        public StoreAddressConfiguration()
        {
            ToTable("THStoreAddress");

            HasKey(x => x.Code).
            Property(x => x.Code).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StoreCode).HasColumnName("Code_Store").IsRequired();
            Property(x => x.AddressIdentifier).HasColumnName("AddressIdentifier_StoreAddress").HasMaxLength(50);
            Property(x => x.ContactName).HasColumnName("ContactName_StoreAddress").IsRequired().HasMaxLength(100);
            Property(x => x.ZipCode).HasColumnName("ZipCode_StoreAddress").IsRequired().HasMaxLength(100);
            Property(x => x.Street).HasColumnName("Street_StoreAddress").IsRequired().HasMaxLength(300);
            Property(x => x.Number).HasColumnName("Number_StoreAddress").IsRequired().HasMaxLength(50);
            Property(x => x.Complement).HasColumnName("Complement_StoreAddress").HasMaxLength(100);
            Property(x => x.District).HasColumnName("District_StoreAddress").IsRequired().HasMaxLength(100);
            Property(x => x.City).HasColumnName("City_StoreAddress").IsRequired().HasMaxLength(100);
            Property(x => x.State).HasColumnName("State_StoreAddress").IsRequired().HasMaxLength(2);
            Property(x => x.PhoneNumber1).HasColumnName("PhoneNumber1_StoreAddress").IsRequired().HasMaxLength(40);
            Property(x => x.PhoneNumber2).HasColumnName("PhoneNumber2_StoreAddress").HasMaxLength(40);
            Property(x => x.PhoneNumber3).HasColumnName("PhoneNumber3_StoreAddress").HasMaxLength(40);
            Property(x => x.Status).HasColumnName("Status_StoreAddress").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_StoreAddress").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_StoreAddress").IsRequired();
        }
    }
}
