using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IRefreshTokenRepository : IRepository, IRepositoryAdd<RefreshToken>, IRepositoryDelete<RefreshToken>, IRepositoryGet<RefreshToken, String>
    {
    }
}
