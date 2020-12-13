using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class AuditLogsConfiguration : EntityTypeConfiguration<AuditLogs>
    {
        public AuditLogsConfiguration()
        {
            ToTable("THAuditLogs");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_AuditLogs");
            Property(x => x.AuditLogsType).HasColumnName("Type_AuditLogs");
            Property(x => x.ReferenceEntity).HasColumnName("ReferenceEntity_AuditLogs");
            Property(x => x.ReferenceCode).HasColumnName("ReferenceCode_AuditLogs");
            Property(x => x.OldData).HasColumnName("OldData_AuditLogs");
            Property(x => x.NewData).HasColumnName("NewData_AuditLogs");
            Property(x => x.ApplicationStoreCode).HasColumnName("Code_ApplicationStore");
            Property(x => x.AccountCode).HasColumnName("Code_Account");
            Property(x => x.SaveDate).HasColumnName("SaveDate_AuditLogs");
        }
    }
}
