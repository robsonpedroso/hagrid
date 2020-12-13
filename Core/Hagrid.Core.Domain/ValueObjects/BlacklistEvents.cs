using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.ValueObjects
{
    public class BlacklistEvents
    {
        [NotMapped]
        public string Reason { get; set; }

        [NotMapped]
        public Guid BlockedBy { get; set; }

        [NotMapped]
        public DateTime SaveDate { get; set; }

        [NotMapped]
        public bool Blocked { get; set; }

    }
}
