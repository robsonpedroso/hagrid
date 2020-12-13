using Newtonsoft.Json;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using DO = Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.DTO
{
    public class MetadataField
    {
        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("json_id")]
        public string JsonId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("format")]
        public int Format { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("validator")]
        public MetadataValidator Validator { get; set; }

        [JsonProperty("save_date")]
        public DateTime SaveDate { get; set; }

        [JsonProperty("is_synchronized")]
        public bool? IsSynchronized { get; set; }

        public MetadataField() { }

        public MetadataField(DO.MetadataField field)
        {
            this.Code = field.Code;
            this.JsonId = field.JsonId;
            this.Name = field.Name;
            this.Type = field.Type.AsInt();
            this.Format = field.Format.AsInt();
            this.Validator = !field.Validator.IsNull() ? field.Validator.GetResult() : null;
            this.SaveDate = field.SaveDate;
        }

        public MetadataField(DO.MetadataField field, BaseMetadata metadata)
            : this(field)
        {
            switch (field.Format)
            {
                case FormatType.Integer:

                    if (metadata.Value.IsNullOrWhiteSpace())
                        Value = null;
                    else
                        Value = metadata.Value.AsLong();

                    break;
                case FormatType.Decimal:

                    if (metadata.Value.IsNullOrWhiteSpace())
                        Value = null;
                    else
                    {
                        decimal _value;

                        decimal.TryParse(metadata.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _value);

                        Value = _value;
                    }

                    break;
                case FormatType.Date:
                    var value = metadata.Value.AsDateTime();

                    if (metadata.Value.IsNullOrWhiteSpace() || value == DateTime.MinValue)
                        Value = null;
                    else
                        Value = value;

                    break;
                case FormatType.Boolean:

                    if (metadata.Value.IsNullOrWhiteSpace())
                        Value = null;
                    else
                        Value = metadata.Value.AsBool();

                    break;
                default:
                    Value = metadata.Value;
                    break;
            }
        }
    }
}