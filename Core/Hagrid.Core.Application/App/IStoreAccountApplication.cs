using System;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IStoreAccountApplication : IBaseApplication
    {
        DTO.StoreAccount Save(DTO.StoreAccount storeAccount);
        DTO.SearchResult Search(DTO.SearchFilterStoreAccount filter);
        DTO.StoreAccount Get(Guid storeCode, Guid accountCode);
        void Delete(Guid storeCode, Guid accountCode);
    }
}
