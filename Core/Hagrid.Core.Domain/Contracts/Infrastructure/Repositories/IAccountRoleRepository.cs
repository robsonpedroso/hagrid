using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IAccountRoleRepository : 
        IRepository, 
        IRepositorySave<AccountRole>, 
        IRepositoryUpdate<AccountRole>, 
        IRepositoryGet<AccountRole, Guid>,
        IRepositoryDelete<AccountRole>,
        IRepositoryDeleteByCode<Guid>
    {
        AccountRole Get(Guid accountCode, Guid roleCode);
        void Delete(IEnumerable<AccountRole> accountsRole);
    }
}
