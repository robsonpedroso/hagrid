using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class ResetSMSTokenApplication : AccountBaseApplication, IResetSMSTokenApplication
    {
        private readonly IResetSMSTokenService smsTokenService;
        private readonly IResetSMSTokenRepository smsTokenRepository;

        public ResetSMSTokenApplication(IComponentContext context, IResetSMSTokenService smsTokenService, IResetSMSTokenRepository smsTokenRepository, ISmsInfraService smsInfraService)
            : base(context)
        {
            this.smsTokenService = smsTokenService;
            this.smsTokenRepository = smsTokenRepository;
        }

        public DO.ResetSMSToken GetResetPasswordSMSToken(string code)
        {
            return smsTokenService.GetResetPasswordSMSToken(code);
        }

        public void Update(DO.ResetSMSToken token)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    smsTokenRepository.Update(token);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Delete(DO.ResetSMSToken token)
        {
            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    smsTokenRepository.Delete(token);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<DTO.Sms> GetSendedSms(Guid customerCode)
        {
            if (customerCode.IsEmpty())
                throw new ArgumentException("Código do cliente inválido");

            var result = smsTokenService.SearchSended(customerCode);

            return result.Select(sms => new DTO.Sms(sms));
        }
    }
}
