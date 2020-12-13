using System;

namespace Hagrid.Core.Domain.ValueObjects
{
    public abstract class SmallToken
    {
        public Guid? OwnerCode { get; internal set; }
        public DateTime ExpiresUtc { get; internal set; }
    }
}
