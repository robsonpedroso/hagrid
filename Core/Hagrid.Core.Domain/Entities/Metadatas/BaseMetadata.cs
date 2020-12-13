using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public abstract class BaseMetadata : IEntity, IDate, IIsValid
    {
        public Guid Code { get; set; }

        public MetadataField Field { get; set; }

        public string Value { get; set; }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public BaseMetadata() { }

        public BaseMetadata(DTO.MetadataField field)
            : this()
        {
            Field = new MetadataField { JsonId = field.JsonId };
            Value = field.Value.AsString();
        }

        public bool IsValid()
        {
            if (Value.IsNullOrWhiteSpace()) return true;

            var valid = true;

            switch (Field.Format)
            {
                case FormatType.String:
                    valid = true;
                    break;

                case FormatType.Integer:
                    long _valL;
                    valid = long.TryParse(Value, out _valL);
                    break;

                case FormatType.Decimal:
                    float _valf;
                    valid = float.TryParse(Value, out _valf);
                    break;

                case FormatType.Date:
                    DateTime _valD;
                    valid = DateTime.TryParse(Value, out _valD);
                    break;

                case FormatType.Boolean:
                    bool _valB;
                    valid = bool.TryParse(Value, out _valB);
                    break;

                case FormatType.Json:
                    valid = Value.IsValidJson();
                    break;
            }

            if (!valid)
            {
                throw new ArgumentException("Ops, Valor informado no campo '{0}' no metadata não é válido para o tipo do campo '{1}'"
                    .ToFormat(Field.JsonId, Field.Format.GetDescription()));
            }

            return valid;
        }

        public BaseMetadata Create()
        {
            this.Code = Guid.NewGuid();
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            return this;
        }

        public BaseMetadata Update(BaseMetadata metadata)
        {
            this.Value = metadata.Value;
            this.UpdateDate = DateTime.Now;

            return this;
        }

        public static BaseMetadata CreateInstance(MetadataField field, BaseMetadata metadata, Guid? referenceCode = null)
        {
            BaseMetadata result = null;

            if (referenceCode.IsNull()) referenceCode = Guid.Empty;

            switch (field.Type)
            {
                case FieldType.Store:
                    result = new StoreMetadata() { StoreCode = referenceCode.Value };
                    break;
                case FieldType.Account:
                    result = new AccountMetadata() { AccountCode = referenceCode.Value };
                    break;
            }

            result.Field = field;

            if (!metadata.IsNull())
                result.Value = metadata.Value;

            return result;
        }
    }
}