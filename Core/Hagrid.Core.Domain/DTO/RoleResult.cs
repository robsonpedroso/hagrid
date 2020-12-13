using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class RoleResult : Role
    {
        [JsonProperty("permissions")]
        public List<PermissionResult> Permissions { get; set; }

        [JsonProperty("account_roles")]
        public List<AccountRoleResult> AccountRoles { get; set; }
    }
}
