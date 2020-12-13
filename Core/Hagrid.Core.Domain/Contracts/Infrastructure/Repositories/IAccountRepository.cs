using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository, IRepositorySave<Account>, IRepositoryUpdate<Account>, IRepositoryGet<Account, Guid>
    {
        IEnumerable<Account> GetLogin(string login, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        IEnumerable<Account> GetEmail(string login, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        IEnumerable<Account> GetDocument(string login, bool includeCustomer = false, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        IEnumerable<Account> Get(string login, ApplicationStore applicationStore, bool? status = null);

        IEnumerable<Account> GetOffBlacklist(IEnumerable<Guid> codes, Guid storeCode);

        IEnumerable<Account> GetOffBlacklistAndHasPermission(IEnumerable<Guid> codes, Guid storeCode, Guid applicationCode);

        IEnumerable<Account> GetOffBlacklistAndHasPermissionUnified(IEnumerable<Guid> codes, Guid storeCode, Guid applicationCode);

        VO.SearchResult<Account> Search(DTO.SearchFilterAccount filter);

        bool IsMemberExists(Account account, bool withDifferentAccountCode = false);

        bool IsMemberExists(string email, Account account);

        Account Get(Guid id, bool includeCustomer, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        List<AccountRole> GetAccountRoleAccess(Guid accountCode, MemberType? memberType = null);
    }
}
