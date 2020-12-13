using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IAccountApplication : IBaseApplication
    {
        Account Authenticate(string login, string password, ApplicationStore applicationStore, out bool requirePasswordChange, string urlBack = "");
        Account AuthenticateTransferToken(Guid accountCode, Guid clientId);
        bool IsMemberExists(DTO.AccountInput account);
        object AccountsAdminTransferToken(string transferToken);
        object CustomerDashboardTransferToken(string transferToken);

        Account GetAccount(Guid accountCode, Guid clientId, bool includeRole = false);
        DTO.Account Get(Guid accountCode, Guid clientId);
        DTO.Account Get(string document, Guid clientId);
        DTO.Account GetEmail(string email, Guid clientId);
        DTO.SearchResult Search(DTO.SearchFilter filter);
        DTO.Account UnlockUser(Guid code, Guid currentAccount);

        DTO.Account SaveCustomerSimplified(DTO.Account customerSimplified, Guid clientId);
        DTO.Account Save(DTO.Account account, Guid clientId, Guid currentAccount, bool addCurrentAppStore = true, bool simplifiedCustomer = false);
        void ChangeAccountInfo(DTO.AccountInput input, Guid currentAccount);
        void Remove(Guid accountCode, Guid currentAccount);

        void ResetMemberPassword(Guid memberCode, string tokenCode, string newPassword, Guid? clientId = null);
        void ChangeMemberPassword(Guid memberCode, string password, string newPassword, Guid? appStoreCode = null);
        object ValidatePassword(Guid memberCode, string password);
        void UpdatePermission(DTO.AccountPermission accountPermission, PermissionType type, Guid clientId, Guid currentAccount);
        DTO.Account GetDetail(Guid accountCode, Guid currentAccount, string accountEmail, bool blacklist = false, bool role = false);

        DTO.Address Save(DTO.Address address, Guid accountCode);

        void RemoveAddress(Guid accountCode, Guid addressCustomerCode);

        DTO.Address GetByCode(Guid accountCode, Guid addressCustomerCode);

        List<DTO.Address> GetAdresses(Guid code);
        object CustomerDashboardChangePasswordToken(string token);
        object CustomerDashboardChangePassword(DTO.ChangePassword changePassword);

        DTO.ResetSMSToken GetTokenSMS(DTO.AccountInput accountInput, Guid clientId);

        void ResetAccount(DTO.AccountInput input, Guid currentAccount);

        void LinkAccountRole(Guid accountCode, Guid roleCode, bool isRKAdmin, Guid clientId);
        void UnLinkAccountRole(Guid accountCode, Guid roleCode, bool isRKAdmin, Guid clientId);
        IEnumerable<DTO.Role> GetRole(Guid accountCode, Guid clientId);
    }
}