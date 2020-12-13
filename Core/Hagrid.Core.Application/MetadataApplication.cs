using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class MetadataApplication : AccountBaseApplication, IMetadataApplication
    {
        private IMetadataFieldRepository metadataFieldRepository;
        private IMetadataService metadataService;

        public MetadataApplication(IComponentContext context, IMetadataService metadataService, IMetadataFieldRepository metadataFieldRepository)
            : base(context)
        {
            this.metadataService = metadataService;
            this.metadataFieldRepository = metadataFieldRepository;
        }

        public DTO.MetadataField Save(DTO.MetadataField field)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var _field = new MetadataField(field);

                    _field = metadataService.SaveField(_field);

                    return new DTO.MetadataField(_field);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public DTO.MetadataField Get(Guid code)
        {
            var field = metadataFieldRepository.Get(code);

            if (field.IsNull())
                throw new ArgumentException("Nenhum campo encontrado com o código informado");

            return new DTO.MetadataField(field);
        }

        public DTO.SearchResult Search(DTO.SearchFilter filter)
        {
            var result = metadataFieldRepository.Search(filter as DTO.SearchFilterMetadataField);

            var fields = result.Results.Select(d => new DTO.MetadataField(d)).ToList();

            return new DTO.SearchResult(fields).SetResult<MetadataField>(result);
        }

        public void Remove(Guid code)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var field = metadataFieldRepository.Get(code);

                    if (field.IsNull())
                        throw new ArgumentException("Nenhum campo encontrado com o código informado");

                    var metadata = BaseMetadata.CreateInstance(field, null);

                    var exists = metadataService.HasValueByJsonId(metadata);

                    if (exists)
                        throw new ArgumentException("Não é possível remover esse campo, pois já existem valores vinculados ao mesmo");

                    metadataFieldRepository.Delete(field);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void SaveValue(FieldType type, Guid referenceCode, IEnumerable<DTO.MetadataField> metadatas)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var _metadatas = metadatas.Select(field =>
                    {
                        var _field = new MetadataField(field);

                        var metadata = BaseMetadata.CreateInstance(_field, null, referenceCode);
                        metadata.Value = field.Value.AsString();

                        return metadata;

                    }).ToList();

                    metadataService.SaveValue(_metadatas);
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}