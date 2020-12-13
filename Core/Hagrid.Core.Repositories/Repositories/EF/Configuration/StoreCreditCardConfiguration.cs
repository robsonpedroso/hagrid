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
    public class StoreCreditCardConfiguration : EntityTypeConfiguration<StoreCreditCard>
    {
        public StoreCreditCardConfiguration()
        {
            ToTable("THStoreCreditCard");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_StoreCreditCard");
            Property(x => x.StoreCode).HasColumnName("StoreCode_StoreCreditCard").IsRequired();

            Property(x => x.StoreName).HasColumnName("StoreName_StoreCreditCard").IsRequired().HasMaxLength(300);
            Property(x => x.CNPJ).HasColumnName("CNPJ_StoreCreditCard").IsRequired().HasMaxLength(14);
            Property(x => x.Number).HasColumnName("Number_StoreCreditCard").IsRequired().HasMaxLength(300);
            Property(x => x.Holder).HasColumnName("Holder_StoreCreditCard").IsRequired().HasMaxLength(300);
            Property(x => x.ExpMonth).HasColumnName("ExpMonth_StoreCreditCard").IsRequired().HasMaxLength(300);
            Property(x => x.ExpYear).HasColumnName("ExpYear_StoreCreditCard").IsRequired().HasMaxLength(300);
            Property(x => x.SecurityCode).HasColumnName("SecurityCode_StoreCreditCard").IsRequired().HasMaxLength(300);
            Property(x => x.Document).HasColumnName("Document_StoreCreditCard").IsRequired().HasMaxLength(14);

            Property(x => x.SaveDate).HasColumnName("SaveDate_StoreCreditCard").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_StoreCreditCard").IsRequired();
        }
    }
}
