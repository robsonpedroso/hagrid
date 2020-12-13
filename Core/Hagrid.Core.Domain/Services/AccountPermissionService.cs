using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Services
{
    public class AccountPermissionService : IAccountPermissionService
    {
        IAccountPermissionRepository accPermissionRep;
        ILockedUpMemberPolicy lockedUp;
        public AccountPermissionService(IAccountPermissionRepository accPermissionRep,
            ILockedUpMemberPolicy lockedUp)
        {
            this.accPermissionRep = accPermissionRep;
            this.lockedUp = lockedUp;
        }

        public Account Get(Guid accountCode, ApplicationStore applicationStore, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            IQueryable<Account> account = accPermissionRep.Get(accountCode, includeMetadata, includeRole, includeBlacklist, includeApplication);

            return Get(account, applicationStore).FirstOrDefault();
        }

        public List<Account> Get(string login, ApplicationStore applicationStore, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false)
        {
            IQueryable<Account> account = accPermissionRep.Get(login, includeMetadata: includeMetadata, includeRole: includeRole, includeBlacklist: includeBlacklist, includeApplication: includeApplication);

            return Get(account, applicationStore);
        }

        public List<Account> Get(IQueryable<Account> account, ApplicationStore applicationStore)
        {
            List<Account> result = null;

            if (!account.IsNull())
            {
                if (applicationStore.Application.MemberType == MemberType.Consumer)
                {
                    var _accounts = account.ToList();

                    if (!_accounts.IsNull())
                    {
                        _accounts.ForEach(acc =>
                        {
                            if (lockedUp.Validate(acc, false) &&
                                acc.Status == true &&
                                acc.Removed == false &&
                                (!acc.Document.IsNullOrWhiteSpace() || !acc.Customer.IsNull()))
                            {
                                acc.ConnectApp(applicationStore);
                            }
                        });

                        result = _accounts;
                    }
                }
                else if (applicationStore.Application.AuthType == AuthType.Distributed)
                {
                    result = account.Where(a =>
                                a.AccountRoles.Any(ar =>
                                    ar.Status == true &&
                                    ar.Role.Status == true &&
                                    ar.Role.StoreCode == applicationStore.StoreCode && ar.Role.Store.Status == true &&
                                    ar.Role.Permissions.Any(
                                        p =>
                                        p.Status == true &&
                                        p.Resource.ApplicationCode == applicationStore.ApplicationCode &&
                                        p.Resource.Application.Status == true
                                        )
                                )
                            ).ToList();
                }
                else if (applicationStore.Store.IsMain)
                {
                    result = account.Where(a =>
                                a.AccountRoles.Any(ar =>
                                    ar.Status == true &&
                                    ar.Role.Status == true &&
                                    ar.Role.Store.Status == true &&
                                    ar.Role.Permissions.Any(
                                        p =>
                                        p.Status == true &&
                                        p.Resource.ApplicationCode == applicationStore.ApplicationCode &&
                                        p.Resource.Application.Status == true
                                        )
                                )
                            ).ToList();
                }

            }

            return result;
        }

        public Account Get(Guid currentAccount, Guid accountCodeToUpdate, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false)
        {
            if (currentAccount.IsEmpty())
                throw new ArgumentException("Você não possui permissão para executar essa ação");

            Account accountUpdate = accPermissionRep.Get(accountCodeToUpdate,
                    includeMetadata: includeMetadata,
                    includeRole: includeRole,
                    includeBlacklist: includeBlacklist,
                    includeApplication: true
                )
                .FirstOrDefault();

            if (currentAccount == accountCodeToUpdate)
                return accountUpdate;

            Account accountCurrent = accPermissionRep.Get(currentAccount, includeApplication: true)
                    .Where(a =>
                        a.AccountRoles.Any(ar =>
                            ar.Status == true &&
                            ar.Role.Status == true &&
                            ar.Role.Store.Status == true &&
                            ar.Role.Permissions.Any(p =>
                                p.Status == true &&
                                p.Resource.Application.MemberType == MemberType.Merchant &&
                                p.Resource.Application.Status == true &&
                                p.Resource.Name.ToLower().Contains(VO.Config.PermissionNameResourceData.ToLower()) &&
                                p.Operations == Operations.Edit
                            )
                        )).FirstOrDefault();

            if (accountCurrent.IsNull())
                throw new ArgumentException("Você não possui permissão para executar essa ação");

            return accountUpdate;
        }

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                accPermissionRep
            };
        }
    }
}
