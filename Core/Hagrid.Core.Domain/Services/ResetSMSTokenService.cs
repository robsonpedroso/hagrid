using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Services
{
    public class ResetSMSTokenService : IResetSMSTokenService
    {
        private ISmsInfraService smsTokenInfraService;
        private IResetSMSTokenRepository smsRepository;
        public ResetSMSTokenService(ISmsInfraService smsTokenInfraService, IResetSMSTokenRepository smsRepository)
        {
            this.smsTokenInfraService = smsTokenInfraService;
            this.smsRepository = smsRepository;
        }

        public DO.ResetSMSToken GetResetPasswordSMSToken(string code)
        {
            return smsRepository.Get(code);
        }

        public DO.ResetSMSToken Generate(Customer customer, ApplicationStore applicationStore, string urlBack)
        {
            DO.ResetSMSToken token = null;

            var codeSMS = new Random().Next(222222, 999999).ToString();

            //Send SMS Token
            var phone = customer.Mobile;
            var ZenviaCode = Guid.NewGuid();
            var tokenCode = ZenviaCode.EncryptDES();

            if (phone.IsNull())
                throw new ArgumentException("Telefone celular não encontrado");

            smsTokenInfraService.SendSms(phone.DDD, phone.Number, string.Format(Config.ResetPasswordSMSMessage, codeSMS, applicationStore.Store.Name), ZenviaCode.AsString());

            token = new DO.ResetSMSToken()
            {
                PhoneNumber = string.Format("{0}{1}", phone.DDD, phone.Number),
                Code = tokenCode,
                CodeSMS = codeSMS,
                OwnerCode = customer.Guid,
                UrlBack = urlBack,
                ApplicationStoreCode = applicationStore.Code,
                ZenviaCode = ZenviaCode
            };

            smsRepository.Add(token);

            return token;
        }

        public IEnumerable<DO.Sms> SearchSended(Guid customerCode)
        {
            var listSms = smsRepository.ListByOwnerCode(customerCode);
            var result = new List<DO.Sms>();

            listSms.ForEach(x =>
            {
                result.Add(smsTokenInfraService.SearchSended(x.ZenviaCode));
            });

            return result;
        }

        private void DeleteAllByOwner(Guid ownerCode)
        {
            var tokens = smsRepository.ListByOwnerCode(ownerCode);

            if (tokens != null)
                smsRepository.DeleteMany(tokens);
        }

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                smsRepository
            };
        }

    }
}
