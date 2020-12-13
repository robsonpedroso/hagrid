using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IMetadataService : IDomainService
    {
        MetadataField SaveField(MetadataField field);

        ICollection<BaseMetadata> SaveValue(IEnumerable<BaseMetadata> metadatas);

        ICollection<BaseMetadata> GetFieldAndFill(FieldType type, IEnumerable<BaseMetadata> metadatas);

        bool HasValueByJsonId(BaseMetadata metadata);
    }
}