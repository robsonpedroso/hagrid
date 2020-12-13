using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IAccountApplicationStoreRepository : IRepository, IRepositorySave<AccountApplicationStore>, IRepositoryUpdate<AccountApplicationStore>, IRepositoryGet<AccountApplicationStore, Guid>
    {
        AccountApplicationStore Get(Guid accountCode, Guid applicationStoreCode);

        AccountApplicationStore Get(Guid accountCode, Guid applicationCode, Guid storeCode);
    }
}
