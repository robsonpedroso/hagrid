using Hagrid.Core.Domain.Enums;
using DTO = Hagrid.Core.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class PointsBalance
    {
        public CurrencyType Currency { get; set; }
        public decimal AggregatedPoints { get; set; }
        public decimal AggregatedValue { get; set; }
        public decimal BalancePoints { get; set; }
        public decimal BalanceValue { get; set; }
        public decimal LockedPoints { get; set; }
        public decimal LockedValue { get; set; }
        public decimal RevokedPoints { get; set; }
        public decimal RevokedValue { get; set; }
        public decimal ExpiredPoints { get; set; }
        public decimal ExpiredValue { get; set; }

        public PointsBalance() { }
    }
}
