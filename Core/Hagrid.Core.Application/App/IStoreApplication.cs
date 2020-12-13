using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Application.Contracts
{
    public interface IStoreApplication
    {
        IEnumerable<DTO.ApplicationStore> Save(DTO.Store store);

        IEnumerable<DTO.ApplicationStore> ConnectApplications(DTO.Store store);

        IEnumerable<DTO.Store> GetByMember(Guid accountCode, Guid clientId);

        IEnumerable<DTO.Store> GetStore(string term);

        IEnumerable<DTO.Store> GetByCnpj(string cnpj);

        IEnumerable<DTO.StoreAddress> GetStoreAddresses(Guid codeStore);

        DTO.Store GetStore(Guid code, Guid clientId);

        DTO.SearchResult Search(DTO.SearchFilter searchFilter);

        DTO.Store Update(DTO.Store store);

        DTO.Store UploadLogo(byte[] item, Guid storeCode);

        string GetEncriptedStore(Guid code, string phrase);
    }
}
