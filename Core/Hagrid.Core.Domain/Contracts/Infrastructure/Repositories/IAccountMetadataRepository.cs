using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IMetadataRepository<T> : IRepository, IRepositorySave<T>, IRepositoryGet<T, Guid> where T : BaseMetadata
    {
        T Get(T metadata);

        bool HasValueByJsonId(BaseMetadata metadata);
    }
}