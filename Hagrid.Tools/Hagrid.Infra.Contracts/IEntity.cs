using System;

namespace Hagrid.Infra.Contracts
{
    public interface IEntity<TKey>
    {
        TKey Code { get; }
    }

    public interface IEntity : IEntity<Guid> { }

    public interface IEntityCore
    {
        Guid Id { get; }
    }
}