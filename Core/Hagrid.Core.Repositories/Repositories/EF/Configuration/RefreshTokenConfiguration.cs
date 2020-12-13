using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class RefreshTokenConfiguration : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenConfiguration()
        {
            ToTable("THRefreshToken");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_RefreshToken").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasMaxLength(200);
            Property(x => x.OwnerCode).HasColumnName("Code_Owner").IsRequired();
            Property(x => x.ClientCode).HasColumnName("Code_ClientApplication");
            Property(x => x.ApplicationStoreCode).HasColumnName("Code_ApplicationStore");
            Property(x => x.GeneratedUtc).HasColumnName("GeneratedUtc_RefreshToken").IsRequired();
            Property(x => x.ExpiresUtc).HasColumnName("ExpiresUtc_RefreshToken").IsRequired();
            Property(x => x.Ticket).HasColumnName("Ticket_RefreshToken").IsRequired();
        }
    }
}
