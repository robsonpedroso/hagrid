using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IStoreRepository : IRepository, IRepositorySave<Store>, IRepositoryGet<Store, Guid>
    {
        Store Get(Guid id, bool includeApplicationStore = false);
        IEnumerable<Store> Get(string name, bool onlyActive = false);
        IEnumerable<Store> GetByCodes(List<Guid> codes);
        IEnumerable<Store> GetByMember(Guid accountCode, Guid clientId);
        VO.SearchResult<Store> Search(DTO.SearchFilter filter);
        IEnumerable<Store> GetByCnpj(string cnpj);
    }
}
