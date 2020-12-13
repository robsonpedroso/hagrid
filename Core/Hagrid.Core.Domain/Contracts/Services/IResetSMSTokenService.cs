using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VO = Hagrid.Core.Domain.ValueObjects;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IResetSMSTokenService : IDomainService
    {
        DO.ResetSMSToken Generate(DO.Customer customer, DO.ApplicationStore applicationStore, string urlBack);
        DO.ResetSMSToken GetResetPasswordSMSToken(string code);
        IEnumerable<DO.Sms> SearchSended(Guid customerCode);
    }
}
