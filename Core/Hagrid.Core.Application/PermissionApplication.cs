using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class PermissionApplication : AccountBaseApplication, IPermissionApplication
    {
        IPermissionRepository repository;
        IResourceRepository resourceRepository;
        IRoleRepository roleRepository;
        IApplicationStoreRepository appStoreRepository;

        public PermissionApplication(
            IComponentContext context, 
            IPermissionRepository repository, 
            IResourceRepository resourceRepository, 
            IRoleRepository roleRepository,
            IApplicationStoreRepository appStoreRepository)
            : base(context)
        {
            this.repository = repository;
            this.resourceRepository = resourceRepository;
            this.roleRepository = roleRepository;
            this.appStoreRepository = appStoreRepository;
        }

        public DTO.Permission Save(bool isRKAdmin, Guid clientId, DTO.Permission permission)
        {
            permission.IsValid();
            Permission result;

            var _permission = permission.Transfer(includeRole: true);

            var _resource = resourceRepository.GetByCode(_permission.ResourceCode, null);
            if (_resource.IsNull())
                throw new ArgumentException("Módulo não encontrado");

            var _role = roleRepository.Get(_permission.RoleCode, null);
            if (_role.IsNull())
                throw new ArgumentException("Grupo não encontrado");

            if (!isRKAdmin)
            {
                var appStore = appStoreRepository.GetByClientId(clientId);

                if (appStore.StoreCode != _role.StoreCode || appStore.Application.Code != _resource.ApplicationCode)
                    throw new ArgumentException("Usuário não possui permissão para realizar essa operação");
            }

            _permission.Role = null;
            _permission.RoleCode = _role.Code;
            _permission.Resource = null;
            _permission.ResourceCode = _resource.Code;

            using (var transaction = Connection.BeginTransaction())
            {
                result = repository.Save(_permission);
            }

            return new DTO.Permission(result);
        }

        public DTO.SearchResult Search(bool isRKAdmin, Guid clientId, Guid? roleCode = null, Guid? applicationCode = null, Guid? resourceCode = null, string resourceName = null, int? skip = null, int? take = null)
        {
            DTO.SearchFilterPermission searchFilter = new DTO.SearchFilterPermission();

            searchFilter.ResourceName = resourceName;

            if (roleCode.HasValue)
                searchFilter.RoleCode = roleCode;

            if (resourceCode.HasValue)
                searchFilter.ResourceCode = resourceCode;

            if (skip.HasValue)
                searchFilter.Skip = skip;

            if (take.HasValue)
                searchFilter.Take = take;

            if (isRKAdmin)
            {
                searchFilter.ApplicationCode = applicationCode;
            }
            else
            {
                var appStore = appStoreRepository.GetByClientId(clientId);

                if (appStore != null)
                {
                    searchFilter.ApplicationCode = appStore.Application.Code;
                }
                else
                {
                    throw new ArgumentException("Aplicação inválida");
                }
            }

            var result = repository.Search(searchFilter);

            var permisions = result.Results.Select(p => new DTO.Permission(p));

            return new DTO.SearchResult(permisions).SetResult<Permission>(result);
        }

        public DTO.Permission Get(bool isRKAdmin, Guid clientId, Guid code)
        {
            Guid? appCode = null;
            if (!isRKAdmin)
            {
                var appStore = appStoreRepository.GetByClientId(clientId);
                appCode = appStore.Application.Code;
            }

            var result = repository.Get(code, appCode);
            if (result.IsNull())
                throw new ArgumentException("Permissão não encontrada");

            return new DTO.Permission(result);
        }

        public void Delete(Guid code)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                repository.Delete(code);
            }
        }
    }
}
