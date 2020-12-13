using System;
using VO = Hagrid.Core.Domain.ValueObjects;
using DTO = Hagrid.Core.Domain.DTO;
using System.Configuration;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Entities
{
    public class ResetSMSToken : VO.Token
    {
        public Guid ZenviaCode { get; set; }

        public DateTime? UsedDate { get; set; }

        public string TokenType { get; set; }

        public string PhoneNumber { get; set; }

        public string UrlBack { get; set; }

        public string CodeSMS { get; set; }

        public ResetSMSToken() {
            this.TokenType = "reset_password_sms";
            this.ExpiresUtc = DateTime.UtcNow.AddMinutes(Config.ResetPasswordSMSExpiresUtc);
            this.GeneratedUtc = DateTime.UtcNow;
        }

        public ResetSMSToken(DTO.ResetSMSToken smsToken)
        {
            TokenType = smsToken.TokenType;
            ExpiresUtc = smsToken.ExpiresUtc;
            PhoneNumber = smsToken.PhoneNumber;
        }

        public bool IsExpired()
        {
            return this.ExpiresUtc.CompareTo(DateTime.UtcNow) <= 0;
        }

        public bool IsCodeValid(string code)
        {
            return this.CodeSMS == code;
        }

        public bool IsUsed()
        {
            return this.UsedDate.HasValue;
        }

        public void SetUsed()
        {
            this.UsedDate = DateTime.Now;
        }
    }
}
