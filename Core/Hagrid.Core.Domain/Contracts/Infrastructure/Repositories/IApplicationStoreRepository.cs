using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IApplicationStoreRepository : IRepository, IRepositoryGet<ApplicationStore, Guid>, IRepositorySave<ApplicationStore>, IRepositoryUpdate<ApplicationStore>
    {
        ApplicationStore Get(ClientAuthType clientAuthType, Guid confClient);

        ApplicationStore GetByClientId(Guid clientId);

        ApplicationStore Get(Guid applicationCode, Guid storeCode, bool includeStore = false);

        IEnumerable<ApplicationStore> GetByStoreTypeMain(Guid applicationCode, bool includeStore = false);

        IEnumerable<ApplicationStore> GetByStore(Guid storeCode);

    }
}
