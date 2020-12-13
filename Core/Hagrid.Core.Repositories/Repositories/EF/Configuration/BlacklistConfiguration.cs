using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class BlacklistConfiguration : EntityTypeConfiguration<Blacklist>
    {
        public BlacklistConfiguration()
        {
            ToTable("THBlacklist");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Blacklist");
            Property(x => x.SaveDate).HasColumnName("SaveDate_Blacklist");
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Blacklist");
            Property(x => x.Blocked).HasColumnName("Blocked_Blacklist");
            Property(x => x.StoreCode).HasColumnName("Code_Store").IsOptional();
            Property(x => x.AccountCode).HasColumnName("Code_Account");
            Property(x => x.Object).HasColumnName("Object_Blacklist");
        }
    }
}
