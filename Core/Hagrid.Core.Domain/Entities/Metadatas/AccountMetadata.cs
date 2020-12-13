using System;
using System.Collections.Generic;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public class AccountMetadata : BaseMetadata
    {
        public Account Account { get; set; }

        public Guid AccountCode { get; set; }

        public AccountMetadata() { }

        public AccountMetadata(Guid accountCode, DTO.MetadataField field)
            : base(field)
        {
            AccountCode = accountCode;
        }
    }
}
