using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Entities;
using System;

namespace Hagrid.Core.Infrastructure.Services.SMS
{
    public class SMSInfraService : ISmsInfraService
    {
        public Sms SearchSended(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SendSms(string ddd, string number, string message, string id)
        {
            throw new NotImplementedException();
        }

        public void SendSms(string number, string message, string id)
        {
            throw new NotImplementedException();
        }
    }
}
