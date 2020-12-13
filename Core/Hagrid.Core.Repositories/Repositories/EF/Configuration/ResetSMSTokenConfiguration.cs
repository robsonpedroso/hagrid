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
    public class ResetSMSTokenConfiguration : EntityTypeConfiguration<ResetSMSToken>
    {
        public ResetSMSTokenConfiguration()
        {
            ToTable("THResetSMSToken");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_ResetSMSToken");
            Property(x => x.OwnerCode).HasColumnName("Code_Owner").IsRequired();
            Property(x => x.ApplicationStoreCode).HasColumnName("Code_ApplicationStore");
            Property(x => x.CodeSMS).HasColumnName("CodeSMS_ResetSMSToken");
            Property(x => x.PhoneNumber).HasColumnName("PhoneNumber_ResetSMSToken");
            Property(x => x.UrlBack).HasColumnName("UrlBack_ResetSMSToken").HasMaxLength(1024);
            Property(x => x.TokenType).HasColumnName("TokenType_ResetSMSToken");
            Property(x => x.GeneratedUtc).HasColumnName("GeneratedUtc_ResetSMSToken").IsRequired();
            Property(x => x.ExpiresUtc).HasColumnName("ExpiresUtc_ResetSMSToken").IsRequired();
            Property(x => x.ZenviaCode).HasColumnName("ZenviaCode_ResetSMSToken").IsRequired();
            Property(x => x.UsedDate).HasColumnName("UsedDate_ResetSMSToken");
        }
    }
}
