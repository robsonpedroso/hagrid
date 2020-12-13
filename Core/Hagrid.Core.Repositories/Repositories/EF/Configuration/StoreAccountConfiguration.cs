using Hagrid.Core.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class StoreAccountConfiguration : EntityTypeConfiguration<StoreAccount>
    {
        public StoreAccountConfiguration()
        {
            ToTable("THStoreAccount");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_StoreAccount");

            Property(x => x.StoreCode).HasColumnName("Code_Store").IsRequired();
            Property(x => x.AccountCode).HasColumnName("Code_Account").IsRequired();

            HasRequired(r => r.Store)
                .WithMany(s => s.StoreAccounts)
                .HasForeignKey(r => r.StoreCode);

            HasRequired(r => r.Account)
                .WithMany(s => s.StoreAccounts)
                .HasForeignKey(r => r.AccountCode);

            Property(x => x.SaveDate).HasColumnName("SaveDate_StoreAccount");
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_StoreAccount");
        }
    }
}
