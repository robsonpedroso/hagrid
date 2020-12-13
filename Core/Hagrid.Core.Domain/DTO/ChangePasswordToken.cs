using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class ChangePasswordToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
