using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.Entities
{
    public class Store : IEntity, IStatus, IIsValid
    {
        public Guid Code { get; set; }
        public String Name { get; set; }
        public String Cnpj { get; set; }
        public bool IsMain { get; set; }
        public ICollection<StoreAddress> Addresses { get; set; }
        public ICollection<StoreMetadata> Metadata { get; set; }
        public bool Status { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<ApplicationStore> ApplicationsStore { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<StoreAccount> StoreAccounts { get; set; }

        public string GetLogoURL()
        {
            return GetLogoURL(Code);
        }

        public static string GetLogoURL(Guid code)
        {
            var accountsSiteURL = Config.AccountsSiteURL;

            return string.Format("{0}/{1}/{2}.png", accountsSiteURL, Properties.ClientAppLogoLocation, code.ToString().ToLowerInvariant());
        }

        public Store()
        {
            IsMain = false;
        }

        public Store(DTO.Store store)
        {
            this.Code = !store.Code.IsEmpty() ? store.Code : Guid.NewGuid();
            this.Name = store.Name;
            this.Cnpj = store.Cnpj;
            this.Status = true;
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;

            if (!store.Addresses.IsNull())
                this.Addresses = store.Addresses.Select(x => new StoreAddress(x)).ToList();
        }

        public bool IsValid()
        {
            if (this.Code.IsEmpty())
                throw new ArgumentException("É necessário enviar o código da loja");

            if (!Cnpj.IsNullOrWhiteSpace() && ((Cnpj.Length <= 11 && !Cnpj.IsValidCPF()) || (Cnpj.Length > 11 && !Cnpj.IsValidCNPJ())))
                throw new ArgumentException("CNPJ inválido");

            if (Name.IsNullOrWhiteSpace())
                throw new ArgumentException("Nome de loja inválido");

            foreach (var item in Addresses)
            {
                if (item.IsNull())
                    throw new ArgumentException("Nenhum endereço fornecido para a loja");

                if (item.ContactName.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem contato");

                if (item.ZipCode.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem CEP");

                if (item.Street.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem logradouro");

                if (item.Number.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem número");

                if (item.City.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem cidade");

                if (item.State.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem estado");
            }

            return true;
        }

        public virtual void SaveLogo(byte[] item)
        {
            using (MemoryStream mStream = new MemoryStream(item))
            {
                Image image;
                try
                {
                    image = Image.FromStream(mStream);
                }
                catch
                {
                    throw new ArgumentException("Arquivo não pode ser convertido para imagem!");
                }

                var dirFileLogo = string.Concat(Config.DirImages, "\\client-apps\\logo");

                System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                var mime = codecs.First(codec => codec.FormatID == image.RawFormat.Guid).MimeType;

                if (!mime.Equals("image/png"))
                    throw new ArgumentException("Formato da imagem incorreto!");

                var extension = "png";
                var nameFileLogo = string.Format("{0}.{1}", this.Code.ToString(), extension);
                var logopng = string.Format(@"{0}\{1}", dirFileLogo.ToLower(), nameFileLogo);

                if (File.Exists(logopng))
                    File.Delete(logopng);

                image.Save(logopng);
            }
        }
    }
}