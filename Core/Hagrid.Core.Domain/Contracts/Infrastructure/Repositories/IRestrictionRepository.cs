using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IRestrictionRepository : IRepository, IRepositorySave<Restriction>, IRepositoryUpdate<Restriction>, IRepositoryGet<Restriction, Guid>
    {
        VO.SearchResult<Restriction> Search(DTO.SearchFilterRestriction filter);
        void Delete(Guid code);
        void Delete(IEnumerable<Restriction> restrictions);
    }
}
