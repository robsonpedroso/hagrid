using System;
using System.ComponentModel;

namespace Hagrid.Core.Domain.Enums
{
    [Flags]
    public enum Operations
    {
        [Description("Visualizar")]
        View = 1,
        [Description("Inserir")]
        Insert = 2,
        [Description("Editar")]
        Edit = 4,
        [Description("Remover")]
        Remove = 8,
        [Description("Aprovar")]
        Approval = 16
    };
}
