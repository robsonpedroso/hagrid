using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IApplicationStoreService : IDomainService
    {
        bool Authenticate(string clientId, string clientSecret, string reqOrigin, out string message, out ApplicationStore applicationStore);

        bool Exists(string clientId);

        ApplicationStore Save(Application application, Store store, string[] jsAllowedOrigins);

        List<ApplicationStore> Get(string[] applications, Guid storeCode);

        ApplicationStore Get(ApplicationStore applicationStore, Guid storeCode);

        List<ApplicationStore> GetByStoreTypeMain(string[] applications);

        List<ApplicationStore> CreateAppStore(Store store);
    }
}
