using Hagrid.Core.Domain.Entities;
using System;
using System.Configuration;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.ValueObjects
{
    public class ResetPasswordToken : Token
    {
        public Guid ClientCode { get; set; }

        public String UrlBack { get; set; }

        internal ResetPasswordToken() { }

        public ResetPasswordToken(Guid ownerCode, Guid applicationStoreCode, string urlBack = "")
        {
            if (ownerCode.Equals(Guid.Empty))
                throw new ArgumentNullException("O parâmetro ownerCode não pode ser nulo");

            if (applicationStoreCode.Equals(Guid.Empty))
                throw new ArgumentNullException("O parâmetro applicationStoreCode não pode ser nulo");

            OwnerCode = ownerCode;
            ApplicationStoreCode = applicationStoreCode;

            Code = Guid.NewGuid().EncryptDES();
            GeneratedUtc = DateTime.UtcNow;
            ExpiresUtc = DateTime.UtcNow.AddHours(Config.ChangePasswordTokenExpirationTimeInHours);

            if (!urlBack.IsNullorEmpty())
            {
                var AccountsSiteURL = Config.AccountsSiteURL;
                UrlBack = string.Format("{0}/{1}/?ub={2}", AccountsSiteURL, Properties.LoginPageLocation, urlBack).URLEncode();
            }
        }

        public bool IsExpired()
        {
            return this.ExpiresUtc.CompareTo(DateTime.UtcNow) <= 0;
        }
    }
}
