using Newtonsoft.Json;
using System;

namespace Hagrid.Core.Domain.DTO
{
    public class Sms
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("receive_date")]
        public DateTime? ReceiveDate { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("mobile_operator_name")]
        public string MobileOperatorName { get; set; }

        [JsonProperty("status_code")]
        public string StatusCode { get; set; }

        [JsonProperty("description")]
        public string DetailDescription { get; set; }

        public Sms(Entities.Sms sms)
        {
            this.Code = sms.Code;
            this.ReceiveDate = sms.ReceiveDate;
            this.ShortCode = sms.ShortCode;
            this.MobileOperatorName = sms.MobileOperatorName;
            this.StatusCode = sms.StatusCode;
            this.DetailDescription = sms.DetailDescription;
        }
    }
}
