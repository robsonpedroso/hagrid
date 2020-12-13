using Newtonsoft.Json;

namespace Hagrid.Infra.Providers.Entities
{
    public class RKEmailResponse
    {
        [JsonProperty("email_send")]
        public bool EmailSend { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

     }

}
