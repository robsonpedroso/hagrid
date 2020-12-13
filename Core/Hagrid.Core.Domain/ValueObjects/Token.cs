using Hagrid.Infra.Contracts;
using System;

namespace Hagrid.Core.Domain.ValueObjects
{
    public abstract class Token : SmallToken, IEntity<string>
    {
        public string Code { get; internal set; }
        public Guid? ApplicationStoreCode { get; internal set; }
        public DateTime GeneratedUtc { get; internal set; }
    }
}
