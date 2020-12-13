using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Enums
{
    public enum AuditLogsType
    {
        [Description("Documento")]
        Document = 0,

        [Description("Data de nascimento")]
        Birthdate = 1,

        [Description("Exclusão")]
        Removed = 2,

        [Description("E-mail")]
        Email = 3
    }
}
