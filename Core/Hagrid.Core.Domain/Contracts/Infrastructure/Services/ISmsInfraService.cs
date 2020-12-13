using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface ISmsInfraService
    {
        void SendSms(string ddd, string number, string message, string id);
        void SendSms(string number, string message, string id);
        Domain.Entities.Sms SearchSended(Guid id);
    }
}
