using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Enums
{
    public enum RequisitionStatus
    {
        [Description("Pendente")]
        Pending = 0,

        [Description("Processando")]
        Processing = 1,

        [Description("Sucesso")]
        Success = 2,

        [Description("Falha")]
        Failure = 3
    }
}
