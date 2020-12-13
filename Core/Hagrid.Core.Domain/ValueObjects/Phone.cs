using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.ValueObjects
{
    public class Phone
    {
        public string CodeCountry { get; set; }

        public PhoneType PhoneType { get; set; }

        public string DDD { get; set; }

        public string Number { get; set; }

        public string Extension { get; set; }
    }
}
