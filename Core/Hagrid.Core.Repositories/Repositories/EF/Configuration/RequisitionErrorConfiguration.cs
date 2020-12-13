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
    public class RequisitionErrorConfiguration : EntityTypeConfiguration<RequisitionError>
    {
        public RequisitionErrorConfiguration()
        {
            ToTable("THRequisitionError");

            HasKey(x => x.Code);
            HasRequired(x => x.Requisition).WithMany(p=>p.RequisitionErrors).Map(m => m.MapKey("Code_Requisition")).WillCascadeOnDelete(false);
            Property(x => x.ObjectSerialized).HasColumnName("Object_RequisitionError").IsRequired();
            
            #region " Ignore "
            Ignore(x => x.ErrorMessages);
            Ignore(x => x.Line);
            Ignore(x => x.Email);
            Ignore(x => x.Name);
            #endregion
        }
    }
}
