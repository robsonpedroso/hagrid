using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Infra.Utils;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hagrid.Core.Domain.Entities
{
    public class MetadataField : IEntity, IDate, IIsValid
    {
        public Guid Code { get; set; }

        public string JsonId { get; set; }

        public string Name { get; set; }

        public FieldType Type { get; set; }

        public FormatType Format { get; set; }

        [NotMapped]
        public MetadataValidator Validator { get; set; }

        [JsonIgnore]
        public string ValidatorSerialized
        {
            get
            {
                if (!Validator.IsNull())
                    return Validator.ToJsonString();

                return null;
            }
            set
            {
                if (!value.IsNullOrWhiteSpace())
                    Validator = value.JsonTo<MetadataValidator>();
            }
        }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public MetadataField()
        {
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }

        public MetadataField(DTO.MetadataField field)
            : this()
        {
            if (!field.Code.IsNull() && !field.Code.Value.IsEmpty())
                this.Code = field.Code.Value;
            else
                this.Code = Guid.NewGuid();

            this.JsonId = field.JsonId.ToLower();
            this.Name = field.Name;
            this.Type = field.Type.ToEnum<FieldType>();
            this.Format = field.Format.ToEnum<FormatType>();

            if (!field.Validator.IsNull())
                this.Validator = field.Validator.Transfer();

            this.JsonId = this.JsonId
                .TakeSpecialCharactersOff(true, true)
                .Replace(" ", "_")
                .Replace("/", "_")
                .Replace("-", "_")
                .Replace(".", "_");
        }

        public bool IsValid()
        {
            if (Name.IsNullOrWhiteSpace())
                throw new ArgumentException("O nome do campo está inválido");

            if (JsonId.IsNullOrWhiteSpace())
                throw new ArgumentException("O JSON ID do campo está inválido");

            return true;
        }
    }
}