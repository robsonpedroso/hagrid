using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class TransferTokenConfiguration : EntityTypeConfiguration<TransferToken>
    {
        public TransferTokenConfiguration()
        {
            ToTable("THTransferToken");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_TransferToken");
            Property(x => x.OwnerCode).HasColumnName("OwnerCode_TransferToken").IsRequired();
            Property(x => x.Name).HasColumnName("Name_TransferToken").IsRequired();
            Property(x => x.ClientId).HasColumnName("ClientId_TransferToken").IsRequired();
            Property(x => x.ExpiresUtc).HasColumnName("ExpiresUtc_TransferToken").IsRequired();
        }
    }
}
