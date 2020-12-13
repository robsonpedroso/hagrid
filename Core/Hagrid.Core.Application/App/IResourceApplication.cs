using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IResourceApplication : IBaseApplication
    {
        DTO.SearchResult Search(bool isRKAdmin, Guid clientId, string name = null, Guid? application_code = null, int? skip = null, int? take = null);
        DTO.Resource GetResource(bool isRKAdmin, Guid clientId, Guid code);
        DTO.Resource Save(bool isRKAdmin, Guid clientId, DTO.Resource resource);
        void Delete(Guid code);
        List<object> GetOperations();
    }
}
