using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class CustomerImportConfiguration: EntityTypeConfiguration<CustomerImport>
    {
        public CustomerImportConfiguration()
        {
            ToTable("THCustomerImport");

            HasKey(x => x.AccountCode);
            Property(x => x.AccountCode).HasColumnName("AccountCode_CustomerImport");
            Property(x => x.Email).HasColumnName("Email_CustomerImport");
            Property(x => x.Password).HasColumnName("Password_CustomerImport");
            Property(x => x.NewsLetter).HasColumnName("NewsLetter_CustomerImport");
            Property(x => x.StoreCode).HasColumnName("Code_Store");
            Property(x => x.Status).HasColumnName("Status_CustomerImport");
            Property(x => x.Removed).HasColumnName("Removed_CustomerImport");
            Property(x => x.SaveDate).HasColumnName("SaveDate_CustomerImport");
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_CustomerImport");
            Property(x => x.AddressData).HasColumnName("Address_CustomerImport");
            Property(x => x.QtyWrongsPassword).HasColumnName("QtyWrongsPassword_CustomerImport");
            Property(x => x.LockedUp).HasColumnName("LockedUp_CustomerImport");

            Map<PersonImport>(m => m.Requires("Type_CustomerImport").HasValue((int)CustomerType.Person));
            Map<CompanyImport>(m => m.Requires("Type_CustomerImport").HasValue((int)CustomerType.Company));
            
            #region " Ignore "

            Ignore(x => x.DisplayName);
            Ignore(x => x.DisplayDocument);
            Ignore(x => x.Address);
            Ignore(x => x.StoreName);
            Ignore(x => x.IP);
            Ignore(x => x.LocalLogin);
            
            Ignore(x => x.Type);

            #endregion
        }
    }

    public class PersonImportConfiguration : EntityTypeConfiguration<PersonImport>
    {
        public PersonImportConfiguration()
        {
            Property(x => x.FirstName).HasColumnName("FirstName_CustomerImport");
            Property(x => x.LastName).HasColumnName("LastName_CustomerImport");
            Property(x => x.CPF).HasColumnName("CPF_CustomerImport");
            Property(x => x.RG).HasColumnName("RG_CustomerImport");
            Property(x => x.BirthDate).HasColumnName("BirthDate_CustomerImport");
            Property(x => x.Gender).HasColumnName("Sexo_CustomerImport").HasColumnType("char").HasMaxLength(1);
        }
    }

    public class CompanyImportConfiguration : EntityTypeConfiguration<CompanyImport>
    {
        public CompanyImportConfiguration()
        {
            Property(x => x.CompanyName).HasColumnName("CompanyName_CustomerImport");
            Property(x => x.TradeName).HasColumnName("TradeName_CustomerImport");
            Property(x => x.CNPJ).HasColumnName("CNPJ_CustomerImport");
            Property(x => x.IE).HasColumnName("IE_CustomerImport");
            Property(x => x.IM).HasColumnName("IM_CustomerImport");
        }
    }
    
}
