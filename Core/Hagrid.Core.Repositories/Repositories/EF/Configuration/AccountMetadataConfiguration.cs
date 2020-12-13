using Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class AccountMetadataConfiguration : BaseMetadataConfiguration<AccountMetadata>
    {
        public AccountMetadataConfiguration()
        {
            ToTable("THAccountMetadata");

            Property(x => x.AccountCode).HasColumnName("Code_Account").IsRequired();
        }
    }
}