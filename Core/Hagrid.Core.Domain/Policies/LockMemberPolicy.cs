using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Policies
{
    public class LockMemberPolicy : ILockMemberPolicy
    {
        public LockMemberPolicy() { }

        public bool Validate(Account account)
        {
            return (!account.QtyWrongsPassword.HasValue || account.QtyWrongsPassword.Value < Config.MaximumConsecutiveWrongLoginAttempts);
        }
    }
}
