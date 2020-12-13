using System;
using System.ComponentModel;

namespace Hagrid.Core.Domain.Enums
{
    [Flags]
    public enum DayOfWeekFalgs
    {
        [Description("Domingo")]
        Sunday = 1,
        [Description("Segunda-feira")]
        Monday = 2,
        [Description("Terça-feira")]
        Tuesday = 4,
        [Description("Quarta-feira")]
        Wednesday = 8,
        [Description("Quinta-feira")]
        Thursday = 16,
        [Description("Sexta-feira")]
        Friday = 32,
        [Description("Sábado")]
        Saturday = 64
    };
}
