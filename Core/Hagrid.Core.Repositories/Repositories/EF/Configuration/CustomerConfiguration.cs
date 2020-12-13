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
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("THCustomer");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Customer").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);            
            Property(x => x.Email).HasColumnName("Email_Customer");
            Property(x => x.Password).HasColumnName("Password_Customer");
            Property(x => x.AddressData).HasColumnName("Address_Customer");
            Property(x => x.Status).HasColumnName("Status_Customer");
            Property(x => x.Removed).HasColumnName("Removed_Customer");
            Property(x => x.SaveDate).HasColumnName("SaveDate_Customer");
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Customer");
            Property(x => x.OriginStore).HasColumnName("Code_Store");
            Property(x => x.NewsLetter).HasColumnName("NewsLetter_Customer");

            HasOptional(x => x.Account).WithOptionalDependent(r => r.Customer).Map(m => m.MapKey("Code_Account"));

            Map<Person>(m => m.Requires("Type_Customer").HasValue((int)CustomerType.Person));
            Map<Company>(m => m.Requires("Type_Customer").HasValue((int)CustomerType.Company));

            #region " Ignore "
            Ignore(x => x.Name);
            Ignore(x => x.Addresses);
            Ignore(x => x.Type);
            Ignore(x => x.Guid);
            #endregion
        }
    }

    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            Property(x => x.FirstName).HasColumnName("FirstName_Customer");
            Property(x => x.LastName).HasColumnName("LastName_Customer");
            Property(x => x.Cpf).HasColumnName("CPF_Customer");
            Property(x => x.Rg).HasColumnName("RG_Customer").HasMaxLength(50);
            Property(x => x.BirthDate).HasColumnName("BirthDate_Customer");
            Property(x => x.Gender).HasColumnName("Sexo_Customer").HasColumnType("char").HasMaxLength(1);
        }
    }

    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            Property(x => x.CompanyName).HasColumnName("CompanyName_Customer");
            Property(x => x.TradeName).HasColumnName("TradeName_Customer");
            Property(x => x.Cnpj).HasColumnName("CNPJ_Customer");
            Property(x => x.Ie).HasColumnName("IE_Customer");
            Property(x => x.Im).HasColumnName("IM_Customer");
        }
    }
}
