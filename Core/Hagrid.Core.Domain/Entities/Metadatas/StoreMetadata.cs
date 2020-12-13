using System;
using System.Collections.Generic;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public class StoreMetadata : BaseMetadata
    {
        public Guid StoreCode { get; set; }

        public Store Store { get; set; }

        public StoreMetadata() { }

        public StoreMetadata(Guid storeCode, DTO.MetadataField field)
            : base(field)
        {
            StoreCode = storeCode;
        }
    }
}
