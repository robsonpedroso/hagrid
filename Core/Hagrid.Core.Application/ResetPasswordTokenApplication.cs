using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.ValueObjects;
using System;

namespace Hagrid.Core.Application
{
    public abstract class ResetPasswordTokenApplication : AccountBaseApplication, IResetPasswordTokenApplication
    {   
        private readonly IResetPasswordTokenRepository resetPasswordTokenRepository;
        private readonly IApplicationStoreRepository applicationStoreRepository;

        public ResetPasswordTokenApplication(IComponentContext context, IResetPasswordTokenRepository resetPasswordTokenRepository, IApplicationStoreRepository applicationStoreRepository)
            : base(context)
        {
            this.resetPasswordTokenRepository = resetPasswordTokenRepository;
            this.applicationStoreRepository = applicationStoreRepository;
        }

        public abstract string GenerateResetPasswordToken(string email, Guid clientId, string urlBack = "", int emailTemplateCode = 0);

        public void DeleteResetPasswordToken(ResetPasswordToken token)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    resetPasswordTokenRepository.Delete(token);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public ResetPasswordToken GetResetPasswordToken(string code)
        {
            return resetPasswordTokenRepository.Get(code);
        }

        public ChangePasswordToken CreateChangePasswordToken(Guid accountCode, Guid clientId, string urlBack, bool showMessage)
        {
            var appSto = applicationStoreRepository.GetByClientId(clientId);

            return new ChangePasswordToken(accountCode, appSto.Code, urlBack, showMessage);
        }
    }
}
