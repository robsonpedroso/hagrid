using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class CustomerApplication : AccountBaseApplication, ICustomerApplication
    {
        private IAccountRepository accountRepository;
        private IApplicationStoreRepository applicationStoreRepository;
        private IAccountService accountService;
        private IAccountPermissionService accPermissionService;

        public CustomerApplication(
            IComponentContext context, 
            IAccountRepository accountRepository,
            IApplicationStoreRepository applicationStoreRepository, 
            IAccountService accountService,
            IAccountPermissionService accPermissionService)
            : base(context)
        {
            this.accountRepository = accountRepository;
            this.applicationStoreRepository = applicationStoreRepository;
            this.accountService = accountService;
            this.accountRepository = accountRepository;
            this.accPermissionService = accPermissionService;
        }

        #region "  Is Member Exists "

        public bool IsMemberExistsByEmail(Guid clientId, string email)
        {
            var account = new Account() { Login = email, Email = email };
            return accountRepository.IsMemberExists(account);
        }

        public bool IsMemberExistsByDocument(Guid clientId, string document)
        {
            var account = new Account() { Login = document, Document = document };
            return accountRepository.IsMemberExists(account);
        }

        #endregion
    }
}