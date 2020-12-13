using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IRoleApplication : IBaseApplication
    {
        DTO.SearchResult Search(Guid storeCode, string name, int? skip = null, int? take = null);
        DTO.Role Get(Guid? storeCode, bool isRKAdmin, Guid code, bool detail = false);
        DTO.Role Save(DTO.Role role);
        void Delete(Guid code);
    }
}
