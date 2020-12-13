using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Application
{
    public class RefreshTokenApplication : AccountBaseApplication, IRefreshTokenApplication
    {
        private IRefreshTokenRepository refreshTokenRepository;
        private IRefreshTokenService refreshTokenService;

        public RefreshTokenApplication(IComponentContext context, IRefreshTokenService refreshTokenService, IRefreshTokenRepository refreshTokenRepository)
            : base(context)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.refreshTokenService = refreshTokenService;
        }

        public void Save(RefreshToken token)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    refreshTokenRepository.Add(token);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public RefreshToken Get(string code)
        {
            return refreshTokenService.Get(code);
        }

        public void Delete(RefreshToken token)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    refreshTokenRepository.Delete(token);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
