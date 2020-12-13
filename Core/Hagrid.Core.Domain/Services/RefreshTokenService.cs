using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private IRefreshTokenRepository refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public Entities.RefreshToken Get(string code)
        {
            var refreshToken = refreshTokenRepository.Get(code);

            if (!refreshToken.IsNull() && refreshToken.ExpiresUtc > DateTime.UtcNow)
            {
                return refreshToken;
            }

            return null;
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { 
                refreshTokenRepository
            };
        }

        #endregion


    }
}
