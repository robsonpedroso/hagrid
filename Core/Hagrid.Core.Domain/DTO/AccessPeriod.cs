using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.DTO
{
    public class AccessPeriod
    {
        [JsonProperty("allowed_week_days")]
        public virtual DayOfWeekFalgs AllowedWeekDays { get; set; }

        [JsonProperty("start_time")]
        public virtual TimeSpan StartTime { get; set; }

        [JsonProperty("end_time")]
        public virtual TimeSpan EndTime { get; set; }

        public AccessPeriod() { }

        public AccessPeriod(Entities.AccessPeriod period)
        {
            AllowedWeekDays = period.AllowedWeekDays;
            StartTime = period.StartTime;
            EndTime = period.EndTime;
        }

        public virtual Entities.AccessPeriod Transfer()
        {
            var period = new Entities.AccessPeriod
            {
                AllowedWeekDays = AllowedWeekDays,
                StartTime = StartTime,
                EndTime = EndTime
            };

            return period;
        }

        public virtual void IsValid()
        {
            var validationErros = new List<string>();

            if (StartTime.IsNull())
                validationErros.Add("Horario de início inválido.");

            if (EndTime.IsNull())
                validationErros.Add("Horario de término inválido.");

            if (validationErros.Count <= 0 && StartTime.CompareTo(EndTime) >= 0)
                validationErros.Add("Horario de início não pode ser maior ou igual ao horário de término.");

            var errorMessage = string.Join(" | ", validationErros);

            if (!string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException(errorMessage);

        }
    }
}
