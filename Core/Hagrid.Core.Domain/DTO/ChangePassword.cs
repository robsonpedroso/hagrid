using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class ChangePassword
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("password_new")]
        public string passwordNew { get; set; }
    }
}
