using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class Sms
    {
        public Guid Code { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ShortCode { get; set; }
        public string MobileOperatorName { get; set; }
        public string StatusCode { get; set; }
        public string DetailDescription { get; set; }
    }
}
