using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IAccountPermissionService : IDomainService
    {
        Account Get(Guid accountCode, ApplicationStore applicationStore, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        List<Account> Get(string login, ApplicationStore applicationStore, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        Account Get(Guid currentAccount, Guid accountCodeToUpdate, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false);
    }
}
