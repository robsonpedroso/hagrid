using Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Contracts.Policies
{
    public interface ILockedUpMemberPolicy
    {
        bool Validate(Account member, bool throwException = true);

        bool Validate(Account account, ApplicationStore applicationStore, bool throwException = true);
    }
}
