using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VO = Hagrid.Core.Domain.ValueObjects;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IPermissionRepository : IRepository, IRepositorySave<Permission>, IRepositoryUpdate<Permission>, IRepositoryGet<Permission, Guid>
    {
        VO.SearchResult<Permission> Search(DTO.SearchFilterPermission searchFilter);
        Permission Get(Guid code, Guid? appCode);
        Permission GetPermission(Guid code);
        void Delete(Guid code);
        void Delete(IEnumerable<Permission> permissions);
    }
}
