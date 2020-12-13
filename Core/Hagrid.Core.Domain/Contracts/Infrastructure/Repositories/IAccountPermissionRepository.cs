using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IAccountPermissionRepository : IRepository, IRepositoryUpdate<Account>
    {
        IQueryable<Account> Get(Guid code, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        IQueryable<Account> Get(string login, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);
    }
}
