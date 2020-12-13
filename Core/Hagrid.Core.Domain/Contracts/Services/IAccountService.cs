using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using VO = Hagrid.Core.Domain.ValueObjects;
using DTO = Hagrid.Core.Domain.DTO;
using Ent = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IAccountService : IDomainService
    {
        ILockedUpMemberPolicy lockedUpMemberPolicy { get; set; }

        ILockMemberPolicy lockMemberPolicy { get; set; }

        IPasswordPolicy passwordPolicy { get; set; }

        void Add(Account account, ApplicationStore app, Guid originStore, bool simplifiedCustomer = false);

        void Update(Account registeredAccount, Store currentStore = null, Account newAccount = null);

        Account Get(string login, ApplicationStore applicationStore, string password = "");

        Account Get(string email, string document, ApplicationStore applicationStore, bool checkEmailAndDocument = false, bool isSave = false);

        IEnumerable<Account> CheckEmailAndDocument(string email, string document, ApplicationStore applicationStore, bool throwException);

        Account Get(Guid accountCode, ApplicationStore applicationStore, bool includeMetadata = false, bool includeRole = false, bool includeBlacklist = false, bool includeApplication = false);

        Account GetIfHasPermissionToUpdate(Guid currentAccount, Guid accountCodeToUpdate, bool blackList = false, bool role = false, bool metaData = false);

        Account Authenticate(IEnumerable<Account> accounts, ApplicationStore applicationStore, bool unlockAccount = true);

        void Authenticate(Account account, ApplicationStore applicationStore, string password);

        Account GetAccountDuplicate(IEnumerable<Account> accounts, ApplicationStore app);

        bool IsMemberExists(Account account, bool withDifferentAccountCode = false);

        void UpdateAccountInfo(Account account, string email = null, string document = null, DateTime? birthdate = null);

        VO.AddressCustomer SaveOrUpdateAddress(VO.AddressCustomer address, Guid accountCode);

        void RemoveAddress(Guid accountCode, Guid addressCustomerCode);

        VO.AddressCustomer GetByCode(Guid accountCode, Guid addressCustomerCode);

        List<VO.AddressCustomer> GetAdresses(Guid code);

        Account UnlockUser(Account account);

        IEnumerable<Account> MatchPassword(IEnumerable<Account> accounts, string password);

     }
}
