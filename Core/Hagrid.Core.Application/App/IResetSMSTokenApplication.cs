using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VO = Hagrid.Core.Domain.ValueObjects;
using DTO = Hagrid.Core.Domain.DTO;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Application.Contracts
{
    public interface IResetSMSTokenApplication
    {
        DO.ResetSMSToken GetResetPasswordSMSToken(string code);
        void Delete(DO.ResetSMSToken token);
        void Update(DO.ResetSMSToken token);
        IEnumerable<DTO.Sms> GetSendedSms(Guid customerCode);
    }
}
