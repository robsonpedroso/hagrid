using System;

namespace Hagrid.Infra.Contracts
{
    public interface IDate
    {
        DateTime SaveDate { get; }

        DateTime UpdateDate { get; }
    }
}