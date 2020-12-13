using Autofac;
using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Factories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Factories
{
    public class ResetPasswordTokenFactory : IResetPasswordTokenFactory
    {
        private readonly IComponentContext context;

        public ResetPasswordTokenFactory(IComponentContext context)
            => this.context = context;

        public IResetPasswordTokenService GetResetPasswordTokenService(IResetPasswordTokenOwner owner)
        {
            if (owner is Account)
            {
                return context.ResolveNamed<IResetPasswordTokenService>("Account");
            }
            else
            {
                return context.ResolveNamed<IResetPasswordTokenService>("CustomerImport");
            }
        }
    }
}
