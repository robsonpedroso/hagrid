using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;
using System.Linq;

namespace Hagrid.Core.Domain.Policies
{
    public class LockedUpMemberPolicy : ILockedUpMemberPolicy
    {
        public bool Validate(Account account, bool throwException = true)
        {
            if (throwException && account.LockedUp.HasValue && account.LockedUp.Value.CompareTo(DateTime.Now) > 0)
                throw new LockedUpMemberException(account);
            else if (account.LockedUp.HasValue && account.LockedUp.Value.CompareTo(DateTime.Now) > 0)
                return false;

            return true;
        }

        public bool Validate(Account account, ApplicationStore applicationStore, bool throwException = true)
        {
            bool valid = true;

            if (!account.BlacklistCollection.IsNull() && account.BlacklistCollection.Count > 0)
            {
                if (account.BlacklistCollection.Any(b => (b.StoreCode == null || b.StoreCode.IsEmpty()) && b.Blocked) || account.BlacklistCollection.Any(b => b.StoreCode == applicationStore.Store.Code && b.Blocked))
                    valid = false;

                if (throwException && !valid)
                    throw new LockedUpMemberException(account, "member '{0}' is blocked.");
            }

            return valid;
        }
    }

    public class LockedUpMemberException : Exception
    {
        public LockedUpMemberException(Account account)
        {
            this.account = account;
        }

        public LockedUpMemberException(Account account, string message)
            : this(account)
        {
            this.message = message;
        }

        public Account account { get; private set; }
        private string message = "member '{0}' is locked up.";


        public override string Message { get { return string.Format(message, account.Email); } }
    }
}
