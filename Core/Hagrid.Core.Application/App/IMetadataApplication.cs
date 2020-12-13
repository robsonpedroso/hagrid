using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IMetadataApplication
    {
        DTO.MetadataField Save(DTO.MetadataField field);

        DTO.MetadataField Get(Guid code);

        DTO.SearchResult Search(DTO.SearchFilter filter);

        void Remove(Guid code);

        void SaveValue(FieldType type, Guid referenceCode, IEnumerable<DTO.MetadataField> metadatas);
    }
}