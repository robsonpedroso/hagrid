using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public class StoreCreditCard : IEntity, IIsValid
    {
        public virtual Guid Code { get; set; }

        public virtual Guid StoreCode { get; set; }

        public virtual string CNPJ { get; set; }

        public virtual string StoreName { get; set; }

        public virtual string Number { get; set; }

        public virtual string Holder { get; set; }

        public virtual string ExpMonth { get; set; }

        public virtual string ExpYear { get; set; }

        public virtual string SecurityCode { get; set; }

        public virtual string Document { get; set; }

        public virtual DateTime SaveDate { get; set; }

        public virtual DateTime UpdateDate { get; set; }

        public StoreCreditCard() { }

        public StoreCreditCard(DTO.StoreCreditCard creditcard)
        {
            if (creditcard.IsNull())
                throw new ArgumentException("Cartão de crédito inválido");

            try
            {
                this.StoreCode = creditcard.StoreCode.DecryptDES().AsGuid();
            }
            catch
            {
                throw new ArgumentException("O link utlizado está inválido");
            }
            
            this.Code = Guid.NewGuid();
            this.StoreName = creditcard.StoreName;
            this.CNPJ = creditcard.CNPJ;
            this.Number = creditcard.Number;
            this.Holder = creditcard.Holder;
            this.ExpMonth = creditcard.ExpMonth;
            this.ExpYear = creditcard.ExpYear;
            this.SecurityCode = creditcard.SecurityCode;
            this.Document = creditcard.Document;
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }

        public bool IsValid()
        {
            if (this.StoreCode.IsEmpty())
                throw new ArgumentException("É necessário enviar o código da loja.");

            if (this.StoreName.IsNullOrWhiteSpace())
                throw new ArgumentException("É necessário informar o nome da loja.");

            if (!this.CNPJ.IsValidCNPJ())
                throw new ArgumentException("CNPJ inválido.");

            if (this.Number.IsNullOrWhiteSpace() || this.Number.AsLong() == 0)
                throw new ArgumentException("Número do cartão inválido.");

            if (this.Holder.IsNullOrWhiteSpace())
                throw new ArgumentException("Nome do titular do cartão inválido.");

            if (this.ExpMonth.IsNullOrWhiteSpace() || this.ExpMonth.AsInt() == 0 || this.ExpMonth.AsInt() <= 0 || this.ExpMonth.AsInt() > 12)
                throw new ArgumentException("Mês de expiração inválido.");

            if (this.ExpYear.IsNullOrWhiteSpace() || this.ExpYear.AsInt() == 0 || this.ExpYear.AsInt() < DateTime.Now.Year)
                throw new ArgumentException("Ano de expiração inválido.");

            if (this.ExpYear.AsInt() == DateTime.Now.Year && this.ExpMonth.AsInt() <= DateTime.Now.Month)
                throw new ArgumentException("Data de expiração inválida.");

            if (this.SecurityCode.IsNullOrWhiteSpace() || this.SecurityCode.AsInt() == 0)
                throw new ArgumentException("Código de segurança inválido.");

            if (this.Document.IsNullOrWhiteSpace())
                throw new ArgumentException("Documento do titular do cartão inválido.");

            if (this.Document.Length <= 11 && !this.Document.IsValidCPF())
                throw new ArgumentException("Documento do titular do cartão inválido.");

            if (this.Document.Length > 11 && !this.Document.IsValidCNPJ())
                throw new ArgumentException("Documento do titular do cartão inválido");

            return true;
        }
    }
}