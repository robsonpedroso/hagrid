using Hagrid.Infra.Utils;
using System.ComponentModel;
using DTO = Hagrid.Core.Domain.DTO;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Enums
{
    public enum FieldType
    {
        [Description("Nenhum")]
        None = 0,

        [Description("Loja")]
        Store = 1,

        [Description("Membro")]
        Account = 2,
    }

    public enum FormatType
    {
        [Description("Nenhum")]
        None = 0,

        [Description("Texto")]
        String = 1,

        [Description("Inteiro")]
        Integer = 2,

        [Description("Decimal")]
        Decimal = 3,

        [Description("Booleano")]
        Boolean = 4,

        [Description("JSON")]
        Json = 5,

        [Description("Data/Hora")]
        Date = 6
    }

    public enum ValidatorType
    {
        [Description("Nenhum")]
        None = 0,

        [Description("Lista de valores")]
        [JsonClass(
           typeof(DTO.MetadataValidatorOptions),
           typeof(DO.MetadataValidatorOptions))]
        Options = 1,

        [Description("Formato de Json")]
        [JsonClass(
           typeof(DTO.MetadataValidatorJSchema),
           typeof(DO.MetadataValidatorJSchema))]
        JsonSchema = 2
    }
}
