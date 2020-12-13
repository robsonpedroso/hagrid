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
    public class RequisitionConfiguration : EntityTypeConfiguration<Requisition>
    {
        public RequisitionConfiguration()
        {
            ToTable("THRequisition");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Requisition");

            //HasRequired(x => x.Store).WithMany().HasForeignKey(s => s.StoreCode).WillCascadeOnDelete(false);
            //Property(x => x.StoreCode).HasColumnName("Code_Store").IsRequired();

            HasRequired(x => x.Store).WithMany().Map(m => m.MapKey("Code_Store")).WillCascadeOnDelete(false);

            Property(x => x.Status).HasColumnName("Status_Requisition").IsRequired();
            Property(x => x.ObjectSerialized).HasColumnName("Object_Requisition").IsRequired();

            HasMany<RequisitionError>(s => s.RequisitionErrors)
                .WithRequired(s => s.Requisition);

            Property(x => x.Removed).HasColumnName("Removed_Requisition");
            Property(x => x.SaveDate).HasColumnName("SaveDate_Requisition");
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Requisition");

            Map<FileRequisition>(m => m.Requires("Type_Requisition").HasValue((int)RequisitionType.ImportExternalAccounts));
            Map<DBRequisition>(m => m.Requires("Type_Requisition").HasValue((int)RequisitionType.ImportInternalAccounts));
        }
    }
}
