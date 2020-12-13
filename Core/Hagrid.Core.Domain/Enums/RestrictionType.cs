using Hagrid.Infra.Utils;
using System.ComponentModel;

namespace Hagrid.Core.Domain.Enums
{
    public enum RestrictionType
    {
        [Description("Endereço de IP"), JsonClass(typeof(DTO.IpAddressRestriction))]
        IpAddress = 1,
        [Description("Período de acesso"), JsonClass(typeof(DTO.PeriodRestriction))]
        Period = 2
    }
}
