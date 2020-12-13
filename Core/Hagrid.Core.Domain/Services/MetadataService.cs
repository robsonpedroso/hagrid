using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Autofac;

namespace Hagrid.Core.Domain.Services
{
    public class MetadataService<T> : IMetadataService where T : BaseMetadata
    {
        private IMetadataFieldRepository metadataFieldRepository;
        private IMetadataRepository<T> metadataRepository;
        private IComponentContext context;

        public MetadataService(IMetadataFieldRepository metadataFieldRepository)
        {
            this.metadataFieldRepository = metadataFieldRepository;
        }

        public MetadataService(IMetadataFieldRepository metadataFieldRepository,
            IMetadataRepository<T> metadataRepository,
            IComponentContext context)
        {
            this.metadataFieldRepository = metadataFieldRepository;
            this.metadataRepository = metadataRepository;
            this.context = context;
        }

        public MetadataField SaveField(MetadataField field)
        {
            field.IsValid();

            var exists = metadataFieldRepository.GetByJsonId(field);

            if (!exists.IsNull() && field.Code != exists.Code)
                throw new ArgumentException("Ops, Já existe um campo com esse JSON ID");

            if (!exists.IsNull())
            {
                exists.Name = field.Name;
                exists.Validator = field.Validator;
                exists.UpdateDate = DateTime.Now;

                field = exists;
            }

            field = metadataFieldRepository.Save(field);

            return field;
        }

        public ICollection<BaseMetadata> SaveValue(IEnumerable<BaseMetadata> metadatas)
        {
            var results = new List<BaseMetadata>();

            metadatas.ForEach(metadata =>
            {
                var field = metadataFieldRepository.GetByJsonId(metadata.Field);

                if (!field.IsNull())
                {
                    metadata.Field = field;

                    metadata.IsValid();

                    if (!field.Validator.IsNull())
                    {
                        MetadataValidator _validatorResult = null;

                        if (context.TryResolveNamed(field.Validator.Type.ToLower(), typeof(IMetadataValidatorService), out var validatorInfraService))
                            _validatorResult = ((IMetadataValidatorService)validatorInfraService).Get(metadata);
                        else
                            _validatorResult = field.Validator;

                        var valid = _validatorResult.IsValid(metadata);

                        if (!valid)
                        {
                            throw new ArgumentException("Valor informado no campo '{0}' no metadata não é válido para o tipo de validador '{1}'"
                                .ToFormat(field.JsonId, field.Validator.Type.GetDescription()));
                        }
                    }

                    BaseMetadata _metadata;

                    var metadataDb = metadataRepository.Get(metadata as T);

                    if (metadataDb.IsNull()) //is new meta
                        _metadata = metadata.Create();
                    else
                        _metadata = metadataDb.Update(metadata);

                    metadataRepository.Save(_metadata as T);

                    results.Add(_metadata);
                }
            });

            return results;
        }

        public ICollection<BaseMetadata> GetFieldAndFill(FieldType type, IEnumerable<BaseMetadata> metadatas)
        {
            var _metadatas = new List<BaseMetadata>();

            var fields = metadataFieldRepository.GetByType(type);

            fields.ForEach(field =>
            {
                var metadataDb = !metadatas.IsNull() ? metadatas.FirstOrDefault(m => m.Field.JsonId == field.JsonId) : null;

                if (metadataDb.IsNull())
                    _metadatas.Add(BaseMetadata.CreateInstance(field, metadataDb));
                else
                    _metadatas.Add(metadataDb);
            });

            return _metadatas;
        }

        public bool HasValueByJsonId(BaseMetadata metadata)
        {
            return metadataRepository.HasValueByJsonId(metadata);
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { metadataFieldRepository, metadataRepository };
        }

        #endregion
    }
}
