using Hagrid.Core.Domain.Enums;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public class AccessPeriod
    {
        public virtual DayOfWeekFalgs AllowedWeekDays { get; set; }
        public virtual TimeSpan StartTime { get; set; }
        public virtual TimeSpan EndTime { get; set; }
    }
}
