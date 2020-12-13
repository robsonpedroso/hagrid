using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class RoleApplication : AccountBaseApplication, IRoleApplication
    {
        private readonly IRoleRepository roleRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IResourceRepository resourceRepository;

        private readonly IRoleService roleService;
        private readonly IStoreService storeService;

        public RoleApplication(
            IComponentContext context,
            IRoleService roleService,
            IStoreService storeService,
            IRoleRepository roleRepository,
            IAccountRepository accountRepository,
            IResourceRepository resourceRepository)
            : base(context)
        {
            this.roleService = roleService;
            this.storeService = storeService;

            this.roleRepository = roleRepository;
            this.accountRepository = accountRepository;
            this.resourceRepository = resourceRepository;
        }

        public DTO.Role Save(DTO.Role role)
        {
            role.IsValid();
            Role result;

            var _role = role.Transfer();

            var roleResult = roleRepository.Get(_role.Code);

            if (!roleResult.IsNull())
                _role.Type = roleResult.Type;

            if (!_role.AccountRoles.IsNull() && _role.AccountRoles.Count > 0)
            {
                _role.AccountRoles.ForEach(acc =>
                {
                    var _account = accountRepository.Get(acc.AccountCode);
                    if (_account.IsNull())
                        throw new ArgumentException(string.Format("Usuário não encontrado na base do accounts {0}", acc.AccountCode.AsString()));
                });
            }

            if (!_role.Permissions.IsNull() && _role.Permissions.Count() > 0)
            {
                _role.Permissions.ForEach(p =>
                {
                    if (p.ResourceCode.IsNull() || p.ResourceCode.IsEmpty())
                        throw new ArgumentException("Código do módulo não informado");

                    var resource = resourceRepository.Get(p.ResourceCode);
                    if (resource.IsNull())
                        throw new ArgumentException($"Módulo de código {p.ResourceCode} não foi encontrado");

                });
            }

            var store = storeService.Get(_role.Store.Code);
            if (store.IsNull())
                throw new ArgumentException("Loja não encontrada");

            _role.StoreCode = store.Code;
            _role.Store = null;

            using (var transaction = Connection.BeginTransaction())
            {
                result = roleService.Save(_role);
            }

            return new DTO.Role(result);
        }

        public DTO.SearchResult Search(Guid storeCode, string name, int? skip = null, int? take = null)
        {
            if (storeCode.IsEmpty() && name.IsNullOrWhiteSpace())
                throw new ArgumentException("É necessário informar a loja ou nome do grupo");

            DTO.SearchFilterRole searchFilter = new DTO.SearchFilterRole();

            if (!name.IsNullOrWhiteSpace())
                searchFilter.Name = name;

            if (!storeCode.IsEmpty())
                searchFilter.StoreCode = storeCode;

            if (skip.HasValue)
                searchFilter.Skip = skip;

            if (take.HasValue)
                searchFilter.Take = take;

            var result = roleRepository.Search(searchFilter);

            var roles = result.Results.Select(p => new DTO.Role(p));

            return new DTO.SearchResult(roles).SetResult<Role>(result);
        }

        public DTO.Role Get(Guid? storeCode, bool isRKAdmin, Guid code, bool detail = false)
        {
            if (code.IsEmpty())
                throw new ArgumentException("Código do grupo não informado");

            var result = roleRepository.Get(code, isRKAdmin ? null : storeCode);

            return new DTO.Role(result, detail);
        }

        public void Delete(Guid code)
        {
            var role = roleRepository.Get(code);

            if (role.IsNull())
                throw new ArgumentException("Código do grupo não foi encontrado.");

            if (role.Type == Domain.Enums.RoleType.StoreAdmin)
                throw new ArgumentException("Não é possível excluir o grupo: {0}".ToFormat(role.Name));

            using (var transaction = Connection.BeginTransaction())
            {
                roleRepository.Delete(code);
            }
        }
    }
}
