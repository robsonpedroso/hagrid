using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Enums
{
    public enum ClientAuthType
    {
        [Description("JavaScript")]
        JavaScript = 0,
        [Description("Confidential")]
        Confidential = 1
    };


    public enum AuthType
    {
        Unified = 0,
        Distributed = 1
    }

    public enum MemberType
    {
        Consumer = 0,
        Merchant = 1,
        Both = 2
    }    

    public enum PasswordEncryptionType
    {
        Default = 0,
        PWDCompare = 1,
        Gateway = 2,
        Nexus = 3,
        Reports = 4,
        Search = 5
    }

    public enum PasswordEventLog
    {
        RequestRecovery = 0,
        Recovery = 1,
        Change = 2,
        ResquetRecoryCustomerImport = 4,
        RecoveryCustomerImport = 5
    }

    public enum PermissionType
    {
        Add = 0,
        Remove = 1
    }
}
