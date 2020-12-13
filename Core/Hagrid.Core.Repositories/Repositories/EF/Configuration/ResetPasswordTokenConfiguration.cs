using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class ResetPasswordTokenConfiguration : EntityTypeConfiguration<ResetPasswordToken>
    {
        public ResetPasswordTokenConfiguration()
        {
            ToTable("THResetPasswordToken");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_ResetPasswordToken");
            Property(x => x.OwnerCode).HasColumnName("Code_Owner").IsRequired();
            Property(x => x.ClientCode).HasColumnName("Code_ClientApplication");
            Property(x => x.ApplicationStoreCode).HasColumnName("Code_ApplicationStore");
            Property(x => x.GeneratedUtc).HasColumnName("GeneratedUtc_ResetPasswordToken").IsRequired();
            Property(x => x.ExpiresUtc).HasColumnName("ExpiresUtc_ResetPasswordToken").IsRequired();
            Property(x => x.UrlBack).HasColumnName("UrlBack_ResetPasswordToken").HasMaxLength(1024);
        }
    }
}
