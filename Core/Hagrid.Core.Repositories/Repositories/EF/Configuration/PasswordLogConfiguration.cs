using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class PasswordLogConfiguration : EntityTypeConfiguration<PasswordLog>
    {
        public PasswordLogConfiguration()
        {
            ToTable("THPasswordLog");

            HasKey(x => x.Code);
            Property(x => x.AccountCode).HasColumnName("Code_Account");
            Property(x => x.Code).HasColumnName("Code_PasswordLog");
            Property(x => x.Event).HasColumnName("Event_Account").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_Account").IsRequired();
            Property(x => x.StoreCode).HasColumnName("Code_Store");

           
        }
    }
}
