using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public class CreditCard : IIsValid
    {
        public string Code { get; set; }

        public string Brand { get; set; }

        public string ExpirationMonth { get; set; }

        public string ExpirationYear { get; set; }

        public string HolderName { get; set; }

        public string HolderDocument { get; set; }

        public string CardNumber { get; set; }

        public string Bin { get; set; }

        public string LastDigits { get; set; }

        public string Signature { get; set; }

        public DateTime? Timestamp { get; set; }

        public string Token { get; set; }

        public CreditCard(){}

        public CreditCard(DTO.CreditCard creditCard)
        {
            this.Code = creditCard.Code;
            this.Brand = creditCard.Brand;
            this.ExpirationMonth = creditCard.ExpirationMonth;
            this.ExpirationYear = creditCard.ExpirationYear;
            this.HolderName = creditCard.HolderName;
            this.HolderDocument = creditCard.HolderDocument;
            this.CardNumber = creditCard.CardNumber;
            this.Bin = creditCard.Bin;
            this.LastDigits = creditCard.LastDigits;
            this.Token = creditCard.Token;
            this.Signature = creditCard.Signature;
            this.Timestamp = creditCard.Timestamp;
        }

        public bool IsValid()
        {
            HolderDocument.ClearStrings();

            if (Brand.IsNullOrWhiteSpace())
                throw new ArgumentException("Bandeira do cartão de crédito não informada");

            if (ExpirationMonth.IsNullOrWhiteSpace())
                throw new ArgumentException("Mês de expiração do cartão de crédito não informado");

            int monthAsInt;
            int.TryParse(ExpirationMonth, out monthAsInt);

            if (monthAsInt< 1 || monthAsInt > 12)
                throw new ArgumentException("Mês de expiração do cartão de crédito inválido");

            if (ExpirationYear.IsNullOrWhiteSpace())
                throw new ArgumentException("Ano de expiração do cartão de crédito não informado");

            int yearAsInt;
            int.TryParse(ExpirationYear, out yearAsInt);

            if (yearAsInt < DateTime.Now.Year)
                throw new ArgumentException("Ano de expiração do cartão de crédito inválido");

            if (monthAsInt < DateTime.Now.Month && yearAsInt == DateTime.Now.Year)
                throw new ArgumentException("Mês de expiração do cartão de crédito inválido");

            if (HolderName.IsNullOrWhiteSpace())
                throw new ArgumentException("Nome do titular do cartão de crédito não informado");

            if (HolderDocument.IsNullOrWhiteSpace())
                throw new ArgumentException("Documento do titular do cartão de crédito não informado");

            if (!HolderDocument.IsValidCPF() && !HolderDocument.IsValidCNPJ())
                throw new ArgumentException("Documento do titular do cartão de crédito inválido");

            if (Bin.IsNullOrWhiteSpace())
                throw new ArgumentException("Bin do cartão de crédito não informado");

            if (LastDigits.IsNullOrWhiteSpace())
                throw new ArgumentException("Últimos 4 dígitos do cartão de crédito não informados");

            if (Signature.IsNullOrWhiteSpace())
                throw new ArgumentException("Assinatura do cartão de crédito não informada");

            if(Timestamp.IsNull() || Timestamp == DateTime.MinValue)
                throw new ArgumentException("Timestamp do cartão de crédito não informados");

            if (Token.IsNullOrWhiteSpace())
                throw new ArgumentException("Token do cartão de crédito não informado");

            return true;
        }
    }
}
