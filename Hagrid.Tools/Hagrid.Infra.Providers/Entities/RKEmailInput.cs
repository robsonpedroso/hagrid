using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Hagrid.Infra.Providers.Entities
{
    public class RKEmailInput
    {
        [JsonProperty("to")]
        public List<EmailContact> To { get; set; }

        [JsonProperty("from")]
        public EmailContact From { get; set; }

        [JsonProperty("blind_copy")]
        public List<string> BlindCopy { get; set; }

        [JsonProperty("campaign_sub_type")]
        public int CampaignSubtype { get; set; }

        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("is_real_time")]
        public bool IsRealTime { get; set; }

        [JsonProperty("hashtags")]
        public List<EmailHashTag> Hashtags { get; set; }

        [JsonProperty("attachment")]
        public EmailAttachment Attachment;

        public RKEmailInput() { }

        [JsonObject("email_attachment")]
        public class EmailAttachment
        {
            [JsonProperty("code")]
            public Guid Code { get; set; }

            public byte[] File { get; private set; }

            public string Filename { get; private set; }

            protected string Extension { get; private set; }
        }

        [JsonObject("email_contact")]
        public class EmailContact
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

        }

        [JsonObject("email_hashtag")]
        public struct EmailHashTag
        {
            [JsonProperty("hashtag")]
            public string Hashtag { get; set; }

            [JsonProperty("value")]
            public object Value { get; set; }
        }
    }
}



