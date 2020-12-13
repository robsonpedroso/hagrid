using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class ResourceApplication : AccountBaseApplication, IResourceApplication
    {
        private readonly IResourceRepository repository;
        private readonly IApplicationRepository appRepository;
        private readonly IApplicationStoreRepository appStoreRepository;

        public ResourceApplication(IComponentContext context, IResourceRepository repository, IApplicationRepository appRepository, IApplicationStoreRepository appStoreRepository)
            : base(context)
        {
            this.repository = repository;
            this.appRepository = appRepository;
            this.appStoreRepository = appStoreRepository;
        }


        public DTO.SearchResult Search(bool isRKAdmin, Guid clientId, string name = null, Guid? application_code = null, int? skip = null, int? take = null)
        {
            DTO.SearchFilterResource searchFilter = new DTO.SearchFilterResource();

            if (!name.IsNullOrWhiteSpace())
                searchFilter.Name = name;

            if (skip.HasValue)
                searchFilter.Skip = skip;

            if (take.HasValue)
                searchFilter.Take = take;

            if (isRKAdmin)
                searchFilter.ApplicationCode = application_code;
            else
            {
                var appStore = appStoreRepository.GetByClientId(clientId);
                searchFilter.ApplicationCode = appStore.Application.Code;
            }

            var result = repository.Search(searchFilter);

            var resources = result.Results.Select(r => new DTO.Resource(r, includeApp: true));

            return new DTO.SearchResult(resources).SetResult<Resource>(result);
        }

        public DTO.Resource Save(bool isRKAdmin, Guid clientId, DTO.Resource resource)
        {
            resource.IsValid();
            Resource result;

            var _resource = resource.Transfer();
            
            var res = repository.GetByCode(_resource.Code, null);

            if (!res.IsNull())
                _resource.Type = res.Type;

            if (!_resource.InternalCode.IsNullOrWhiteSpace())
            {
                var _internal = repository.GetByInternalCode(_resource.InternalCode, _resource.Application.Code.Value);

                if (!_internal.IsNull() && _internal.Code != _resource.Code)
                    throw new ArgumentException("Já existe o código interno informado para essa aplicação");
            }
            else
                _resource.InternalCode = string.Empty;

            Domain.Entities.Application app;
            if (isRKAdmin)
            {
                app = appRepository.Get(_resource.Application.Code);

                if (app.IsNull())
                    throw new ArgumentException("Aplicação não encontrada");
            }
            else
            {
                var appStore = appStoreRepository.GetByClientId(clientId);
                if (appStore.IsNull())
                    throw new ArgumentException("Aplicação inválida");

                app = appStore.Application;

                if (app.IsNull())
                    throw new ArgumentException("Aplicação não encontrada");

                if (_resource.Application.Code.Value != app.Code)
                    throw new ArgumentException("Código da aplicação inválido");
            }

            _resource.ApplicationCode = app.Code.Value;
            _resource.Application = null;

            using (var transaction = Connection.BeginTransaction())
            {
                result = repository.Save(_resource);
            }

            return new DTO.Resource(result, includeApp: true);
        }

        public DTO.Resource GetResource(bool isRKAdmin, Guid clientId, Guid code)
        {
            Resource result;
            
            Guid? appCode = null;
            if (!isRKAdmin)
            {
                var appStore = appStoreRepository.GetByClientId(clientId);
                if (appStore.IsNull())
                    throw new ArgumentException("Aplicação inválida");

                appCode = appStore.Application.Code;
            }

            result = repository.GetByCode(code, appCode);
            if (result.IsNull())
                throw new ArgumentException("Módulo não encontrado");

            return new DTO.Resource(result);
        }

        public void Delete(Guid code)
        {
            var resource = repository.GetByCode(code, null);

            if (resource.IsNull())
                throw new ArgumentException("Código do módulo não foi encontrado.");

            if (resource.Type == ResourceType.ApplicationAccess)
                throw new ArgumentException("Não é possível excluir o módulo: {0}".ToFormat(resource.Name));

            using (var transaction = Connection.BeginTransaction())
            {
                repository.Delete(code);
            }
        }

        public List<object> GetOperations()
        {
            var result = new List<object> { };
            var operations = EnumExtension.GetValues<Operations>();

            operations.ForEach(item =>
            {
                result.Add(new
                {
                    code = item.AsInt(),
                    name = item.ToString(),
                    description = item.GetDescription()
                });
            });

            return result;
        }
    }
}
