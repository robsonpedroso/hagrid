using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IStoreAccountRepository : IRepository, IRepositorySave<StoreAccount>, IRepositoryGet<StoreAccount, Guid>
    {
        VO.SearchResult<StoreAccount> Search(DTO.SearchFilterStoreAccount filter);
        void Delete(Guid code);
        void Delete(IEnumerable<StoreAccount> storeAccount);
        StoreAccount Get(Guid storeCode, Guid accountCode);
    }
}
