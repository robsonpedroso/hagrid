using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class AccountRepository : BaseRepository<Account, CustomerContext, Guid>, IAccountRepository
    {
        public AccountRepository(IConnection connection) : base(connection) { }

        #region " Login  "
        public IEnumerable<Account> Get(string login, ApplicationStore applicationStore, bool? status = null)
        {
            IEnumerable<Account> _result = null;

            _result = GetEmail(login);

            if (_result.Count() == 0)
                _result = GetDocument(login);

            if (_result.Count() == 0)
                _result = GetLogin(login);

            return _result;
        }

        public IEnumerable<Account> GetLogin(string login, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            var accounts = (from account in Collection
                            where account.Removed == false &&
                            account.Status &&
                            account.Login == login
                            select account);

            accounts = IncludesResult(
                accounts, 
                includeCustomer: includeCustomer,
                includeMetadata: includeMetadata, 
                includeRole: includeRole, 
                includeBlacklist: includeBlacklist, 
                includeApplication: includeApplication
            );

            return accounts;
        }

        public IEnumerable<Account> GetEmail(string login, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            var accounts = (from account in Collection
                            where account.Removed == false &&
                            account.Status &&
                            account.Email == login
                            select account);

            accounts = IncludesResult(
                accounts,
                includeCustomer: includeCustomer,
                includeMetadata: includeMetadata,
                includeRole: includeRole,
                includeBlacklist: includeBlacklist,
                includeApplication: includeApplication
            );

            return accounts;
        }

        public IEnumerable<Account> GetDocument(string login, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            var accounts = (from account in Collection
                            where account.Removed == false &&
                            account.Status &&
                            account.Document == login
                            select account);

            accounts = IncludesResult(
                accounts,
                includeCustomer: includeCustomer,
                includeMetadata: includeMetadata,
                includeRole: includeRole,
                includeBlacklist: includeBlacklist,
                includeApplication: includeApplication
            );

            return accounts;
        }

        public IEnumerable<Account> GetOffBlacklist(IEnumerable<Guid> codes, Guid storeCode)
        {
#if DEBUG
            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif

            var accounts = Collection.Where(a =>
                                codes.Contains(a.Code) &&
                                !a.BlacklistCollection.Any(b =>
                                    (b.StoreCode == storeCode || b.StoreCode == null) && b.Blocked == true))
                               .Include(a => a.Customer);

            return accounts;
        }

        public IEnumerable<Account> GetOffBlacklistAndHasPermission(IEnumerable<Guid> codes, Guid storeCode, Guid applicationCode)
        {
#if DEBUG
            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif

            var accounts = Collection.Where(a =>
                            codes.Contains(a.Code) &&
                            !a.BlacklistCollection.Any(b =>
                                (b.StoreCode == storeCode || b.StoreCode == null) &&
                                b.Blocked == true) &&
                            a.AccountRoles.Any(ar =>
                                ar.Status == true &&
                                ar.Role.Status == true &&
                                ar.Role.StoreCode == storeCode && ar.Role.Store.Status == true &&
                                ar.Role.Permissions.Any(
                                    p =>
                                    p.Status == true &&
                                    p.Resource.ApplicationCode == applicationCode &&
                                    p.Resource.Application.Status == true
                                    )
                            )
                        )
                        .Include(a => a.AccountRoles.Select(ar => ar.Role.Store))
                        .Include(a => a.AccountRoles.Select(ar => ar.Role.Permissions.Select(p => p.Resource.Application)));

            return accounts;
        }

        public IEnumerable<Account> GetOffBlacklistAndHasPermissionUnified(IEnumerable<Guid> codes, Guid storeCode, Guid applicationCode)
        {
#if DEBUG
            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif

            var _accounts = Collection.Where(a =>
                            codes.Contains(a.Code)
                        )
                        .Include(a => a.BlacklistCollection)
                        .Include(a => a.AccountRoles.Select(ar => ar.Role.Store))
                        .Include(a => a.AccountRoles.Select(ar => ar.Role.Permissions.Select(p => p.Resource.Application))).ToList();

            var _result = _accounts.Where(a => !a.BlacklistCollection.Any(b =>
                                (b.StoreCode == storeCode || b.StoreCode == null) &&
                                b.Blocked == true) &&
                            a.AccountRoles.Any(ar =>
                                ar.Status == true &&
                                ar.Role.Status == true &&
                                ar.Role.Store.Status == true &&
                                ar.Role.Permissions.Any(
                                    p =>
                                    p.Status == true &&
                                    p.Resource.ApplicationCode == applicationCode &&
                                    p.Resource.Application.Status == true
                                    )
                            )
                    );

            return _result;
        }

        #endregion

        public override Account Save(Account obj)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif

            return base.Save(obj);
        }

        public VO.SearchResult<Account> Search(DTO.SearchFilterAccount filter)
        {

#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);
#endif
            IQueryable<Account> result = Collection.AsQueryable();

            if (!filter.Code.IsNull() && !filter.Code.IsEmpty())
                result = result.Where(a => a.Code == filter.Code);

            if (!filter.Document.IsNullOrWhiteSpace())
                result = result.Where(a => a.Document == filter.Document);

            if (!filter.Email.IsNullOrWhiteSpace())
                result = result.Where(a => a.Email == filter.Email);

            result = result.Where(a => a.Removed == false);
            result = result.OrderBy(i => i.SaveDate);

            var count = result.Count();

            result = result.Skip((filter.Skip.Value) * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<Account>(result.ToList(), filter, count);
        }

        public List<AccountRole> GetAccountRoleAccess(Guid accountCode, MemberType? memberType = null)
        {
            var accountsQuery = (from account in Collection
                                 where !account.Removed && account.Code == accountCode
                                 select account)
                                    .Include(a => a.AccountRoles.Select(r => r.Role))
                                    .Include(a => a.AccountRoles.Select(r => r.Role).Select(s => s.Store))
                                    .Include(a => a.AccountRoles.Select(r => r.Role.Permissions.Select(p => p.Resource.Application)))
                                    .FirstOrDefault();

            var accRoles =
                    accountsQuery.AccountRoles.Where(ar =>
                        ar.Role.Name.ToLower().Contains(VO.Config.PermissionNameRole.ToLower()) &&
                        ar.Status &&
                        ar.Role.Status &&
                        ar.Role.Permissions.Any(p =>
                            p.Status &&
                            p.Resource.Application.Status
                        )
                    );

            if (memberType.HasValue)
                accRoles = accRoles.Where(ar => ar.Role.Permissions.Any(p => p.Resource.Application.MemberType == memberType.Value));

            return accRoles.ToList();
        }

        public bool IsMemberExists(Account account, bool withDifferentAccountCode = false)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            IQueryable<Account> accounts = null;

            if (!account.Login.IsNullOrWhiteSpace())
            {
                accounts = from acc in Collection
                        where !acc.Removed && acc.Login == account.Login
                        select acc;
            }

            if(!account.Email.IsNullOrWhiteSpace())
            {
                var accountsByEmail = from acc in Collection
                                   where !acc.Removed && acc.Email == account.Email
                                   select acc;

                accounts = accounts?.Union(accountsByEmail) ?? accountsByEmail;
            }

            if (!account.Document.IsNullOrWhiteSpace())
            {
                var accountsByDocument = from acc in Collection
                                      where !acc.Removed && acc.Document == account.Document
                                      select acc;

                accounts = accounts?.Union(accountsByDocument) ?? accountsByDocument;
            }

            if(withDifferentAccountCode)
            {
                accounts = from acc in accounts
                           where acc.Code != account.Code
                           select acc;
            }

            accounts.Include(a => a.AccountRoles);

            return accounts.Any();
        }

        public bool IsMemberExists(string email, Account account)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif


            var accounts = from acc in Collection
                           where !acc.Removed
                           && (acc.Email == email && acc.Code != account.Code)
                           select acc;

            accounts.Include(a => a.AccountRoles);

            return accounts.Any();
        }

        public Account Get(Guid id, bool includeCustomer, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
#if DEBUG
            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            var accountsQuery = (from account in Collection
                                 where !account.Removed && account.Code == id
                                 select account);

            if (includeCustomer)
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

            var accounts = accountsQuery.FirstOrDefault();

            return accounts;
        }
        
        private IQueryable<Account> IncludesResult(IQueryable<Account> accounts, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            if (includeCustomer)
                accounts = accounts.Include(a => a.Customer);

            if (includeMetadata)
            {
                accounts = accounts
                        .Include(a => a.Metadata)
                        .Include(a => a.Metadata.Select(m => m.Field));
            }

            if (includeRole)
            {
                accounts = accounts.Include(a => a.AccountRoles.Select(r => r.Role));
                accounts = accounts.Include(a => a.AccountRoles.Select(r => r.Role).Select(s => s.Store));
                accounts = accounts.Include(a => a.AccountRoles.Select(r => r.Role.Permissions.Select(p => p.Resource.Application)));
            }

            if (includeBlacklist)
            {
                accounts = accounts.Include(x => x.BlacklistCollection.Select(s => s.Store));
            }

            if (includeApplication)
            {
                accounts = accounts.Include(a => a.AccountApplicationStoreCollection.Select(aps => aps.ApplicationStore.Store));
                accounts = accounts.Include(a => a.AccountApplicationStoreCollection.Select(aps => aps.ApplicationStore.Application));
            }

            return accounts;
        }
    }
}
