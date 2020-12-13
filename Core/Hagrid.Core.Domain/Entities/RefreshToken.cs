using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public class RefreshToken : Token
    {
        public string Ticket { get; set; }

        public Guid ClientCode { get; set; }

        internal RefreshToken() { }

        public RefreshToken(Guid ownerCode, ApplicationStore appStore)
        {
            if (ownerCode.Equals(Guid.Empty))
                throw new ArgumentException("O parâmetro ownerCode não pode ser nulo");

            if (appStore == null)
                throw new ArgumentException("O parâmetro client não pode ser nulo");

            OwnerCode = ownerCode;
            ApplicationStoreCode = appStore.Code;

            Code = Guid.NewGuid().EncryptDES();
            GeneratedUtc = DateTime.UtcNow;
            ExpiresUtc = DateTime.UtcNow.AddMinutes(appStore.Application.RefreshTokenLifeTimeInMinutes);    
        }
    }
}
