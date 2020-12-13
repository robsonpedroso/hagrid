using System;
using System.ComponentModel;

namespace Hagrid.Core.Domain.Enums
{
    public enum ResourceType
    {
        [Description("Padrão")]
        Default = 0,

        [Description("Acesso Padrão")]
        ApplicationAccess = 1,
    };
}
