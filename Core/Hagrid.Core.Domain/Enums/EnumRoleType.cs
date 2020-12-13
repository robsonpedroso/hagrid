using System;
using System.ComponentModel;

namespace Hagrid.Core.Domain.Enums
{
    public enum RoleType
    {
        [Description("Padrão")]
        Default = 0,

        [Description("Administrador")]
        StoreAdmin = 1,
    };
}
