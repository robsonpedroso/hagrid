using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Diagnostics;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class AccountApplicationStoreRepository : BaseRepository<AccountApplicationStore, CustomerContext, Guid>, IAccountApplicationStoreRepository
    {
        public AccountApplicationStoreRepository(IConnection connection) : base(connection) { }

        public AccountApplicationStore Get(Guid accountCode, Guid applicationStoreCode)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            var accountApplicationStore = (from aas in Collection
                            where aas.AccountCode == accountCode &&
                            aas.ApplicationStoreCode == applicationStoreCode
                            select aas).FirstOrDefault();

            return accountApplicationStore;
        }

        public AccountApplicationStore Get(Guid accountCode, Guid applicationCode, Guid storeCode)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            var accountApplicationStore = (from aas in Collection
                                           where aas.AccountCode == accountCode &&
                                           aas.ApplicationStore.ApplicationCode == applicationCode &&
                                           aas.ApplicationStore.StoreCode == storeCode
                                           select aas).FirstOrDefault();

            return accountApplicationStore;
        }
    }
}
