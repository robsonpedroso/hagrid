using Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Contracts.Policies
{
    public interface ILockMemberPolicy
    {
        bool Validate(Account member);
    }
}
