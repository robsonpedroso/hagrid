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
    public interface IRoleRepository : IRepository, IRepositorySave<Role>, IRepositoryUpdate<Role>, IRepositoryGet<Role, Guid>
    {
        VO.SearchResult<Role> Search(DTO.SearchFilterRole filter);
        Role Get(Guid code, Guid? storeCode);
        Role GetByApplication(string appName, Guid storeCode);
        Role GetByApplication(Guid applicationCode, Guid storeCode);
        IEnumerable<Role> ListRoles(Guid storeCode, Guid applicationCode, Guid accountCode);
        void Delete(Guid code);
    }
}
