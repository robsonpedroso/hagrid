using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken, CustomerContext, string>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IConnection connection) : base(connection) { }
    }
}
