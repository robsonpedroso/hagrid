using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class AccountPermissionRepository : BaseRepository<Account, CustomerContext, Guid>, IAccountPermissionRepository
    {
        public AccountPermissionRepository(IConnection connection) : base(connection) { }

        public IQueryable<Account> Get(Guid code, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
#if DEBUG
            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            var accountsQuery = (from account in Collection
                                 where account.Code == code && !account.Removed
                                 select account);

            accountsQuery = accountsQuery.Include(a => a.Customer);

            if (includeMetadata)
            {
                accountsQuery = accountsQuery
                        .Include(a => a.Metadata)
                        .Include(a => a.Metadata.Select(m => m.Field));
            }

            if (includeRole)
            {
                accountsQuery = accountsQuery.Include(a => a.AccountRoles.Select(r => r.Role));
                accountsQuery = accountsQuery.Include(a => a.AccountRoles.Select(r => r.Role).Select(s => s.Store));
                accountsQuery = accountsQuery.Include(a => a.AccountRoles.Select(r => r.Role.Permissions.Select(p => p.Resource.Application)));
            }

            if (includeBlacklist)
            {
                accountsQuery = accountsQuery.Include(x => x.BlacklistCollection.Select(s => s.Store));
            }

            if (includeApplication)
            {
                accountsQuery = accountsQuery.Include(a => a.AccountApplicationStoreCollection.Select(aps => aps.ApplicationStore.Store));
                accountsQuery = accountsQuery.Include(a => a.AccountApplicationStoreCollection.Select(aps => aps.ApplicationStore.Application));
            }

            return accountsQuery;
        }

        public IQueryable<Account> Get(string login, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
#if DEBUG
            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            var accountsQuery = (from account in Collection
                                 where
                                    !account.Removed &&
                                    (
                                        account.Login == login ||
                                        account.Email == login ||
                                        account.Document == login
                                    )
                                 select account);

            accountsQuery = accountsQuery.Include(a => a.Customer);

            if (includeMetadata)
            {
                accountsQuery = accountsQuery
                        .Include(a => a.Metadata)
                        .Include(a => a.Metadata.Select(m => m.Field));
            }

            if (includeRole)
            {
                accountsQuery = accountsQuery.Include(a => a.AccountRoles.Select(r => r.Role));
                accountsQuery = accountsQuery.Include(a => a.AccountRoles.Select(r => r.Role).Select(s => s.Store));
                accountsQuery = accountsQuery.Include(a => a.AccountRoles.Select(r => r.Role.Permissions.Select(p => p.Resource.Application)));
            }

            if (includeBlacklist)
            {
                accountsQuery = accountsQuery.Include(x => x.BlacklistCollection.Select(s => s.Store));
            }

            if (includeApplication)
            {
                accountsQuery = accountsQuery.Include(a => a.AccountApplicationStoreCollection.Select(aps => aps.ApplicationStore.Store));
                accountsQuery = accountsQuery.Include(a => a.AccountApplicationStoreCollection.Select(aps => aps.ApplicationStore.Application));
            }

            return accountsQuery;
        }
    }
}
