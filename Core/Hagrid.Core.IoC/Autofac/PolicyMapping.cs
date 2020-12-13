using Autofac;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Policies;

namespace Hagrid.Core.IoC.Autofac
{
    public static class PolicyMapping
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<LockedUpMemberPolicy>().As<ILockedUpMemberPolicy>();
            builder.RegisterType<LockMemberPolicy>().As<ILockMemberPolicy>();
            builder.RegisterType<PasswordPolicy>().As<IPasswordPolicy>();
        }
    }
}
