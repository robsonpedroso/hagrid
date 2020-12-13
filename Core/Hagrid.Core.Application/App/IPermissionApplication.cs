using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IPermissionApplication : IBaseApplication
    {
        DTO.Permission Save(bool isRKAdmin, Guid clientId, DTO.Permission permission);
        DTO.SearchResult Search(bool isRKAdmin, Guid clientId, Guid? roleCode = null, Guid? applicationCode = null, Guid? resourceCode = null, string resourceName = null, int? skip = null, int? take = null);
        DTO.Permission Get(bool isRKAdmin, Guid clientId, Guid code);
        void Delete(Guid code);
    }
}
