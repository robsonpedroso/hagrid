using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using Hagrid.Core.Domain.Enums;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IMetadataFieldRepository : IRepository, IRepositorySave<MetadataField>, IRepositoryGet<MetadataField, Guid>, IRepositoryDelete<MetadataField>
    {
        SearchResult<MetadataField> Search(DTO.SearchFilterMetadataField filter);

        MetadataField GetByJsonId(MetadataField field);

        IEnumerable<MetadataField> GetByType(FieldType type);
    }
}
