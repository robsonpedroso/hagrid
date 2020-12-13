using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ValueObjects;
using System;

namespace Hagrid.Core.Domain.DTO
{
    public class Application
    {
        public Application() { }

        public Application(Entities.Application app)
        {
            this.Code = app.Code;
            this.Name = app.Name;
            this.Status = app.Status;
            this.RefreshTokenLifeTimeInMinutes = app.RefreshTokenLifeTimeInMinutes;
            this.MemberType = app.MemberType;
            this.AuthType = app.AuthType;
        }

        public Application(Entities.Application app, bool withInformations)
            : this(app)
        {
            if (withInformations)
                this.Informations = app.Informations;
        }

        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }

        [JsonProperty("informations")]
        public ApplicationInformation Informations { get; set; }

        [JsonProperty("refresh_token_life")]
        public int? RefreshTokenLifeTimeInMinutes { get; set; }

        [JsonProperty("member_type")]
        public MemberType? MemberType { get; set; }

        [JsonProperty("auth_type")]
        public AuthType? AuthType { get; set; }
    }
}
