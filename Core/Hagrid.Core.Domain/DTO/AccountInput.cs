using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountInput
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_new")]
        public string EmailNew { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }

        [JsonProperty("document_new")]
        public string DocumentNew { get; set; }

        [JsonProperty("birth_date_new")]
        public DateTime? BirthdateNew { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password_new")]
        public string PasswordNew { get; set; }

        [JsonProperty("url_back")]
        public string UrlBack { get; set; }

        [JsonProperty("show_change_password_message")]
        public bool ShowChangePasswordMessage { get; set; }

        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("member_type")]
        public MemberType MemberType { get; set; }

        public AccountInput()
        {

        }

        public AccountInput(string login, string email, string document)
        {
            this.Login = login;
            this.Email = email;
            this.Document = document;
        }

        public DO.Account Convert()
        {
            return new DO.Account()
            {
                Login = this.Login,
                Email = this.Email,
                Document = this.Document,
                Password = this.PasswordNew.IsNullOrWhiteSpace() ? this.PasswordNew : this.Password
            };
        }
    }
}