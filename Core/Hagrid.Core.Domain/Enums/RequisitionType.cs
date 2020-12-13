using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Enums
{
    public enum RequisitionType
    {
        /// <summary>
        /// Importação de contas internal. Importação via DB.
        /// </summary>
        [Description("Importação Interna")]
        ImportInternalAccounts = 1,

        /// <summary>
        /// Importação de contas externas. Requisição de importação de arquivo txt.
        /// </summary>
        [Description("Importação Externa")]
        ImportExternalAccounts = 2
    }
}
