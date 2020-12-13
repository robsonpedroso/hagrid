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
            {
                this.Addresses = new List<StoreAddress>();
                store.Addresses.ForEach(address =>
                {
                    this.Addresses.Add(new StoreAddress()
                    {
                        AddressIdentifier = address.AddressIdentifier,
                        City = address.City,
                        Code = address.Code,
                        Complement = address.Complement,
                        ContactName = address.ContactName,
                        District = address.District,
                        Number = address.Number,
                        PhoneNumber1 = !address.PhoneNumber1.ToNumber().ToString().IsNullOrWhiteSpace() ?
                                        address.PhoneNumber1.ToNumber().ToString() :
                                        address.Phone.ToNumber().ToString(),
                        PhoneNumber2 = address.PhoneNumber2.ToNumber().ToString(),
                        PhoneNumber3 = address.PhoneNumber3.ToNumber().ToString(),
                        State = address.State,
                        Status = true,
                        Street = address.Street,
                        ZipCode = address.ZipCode,
                        SaveDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    });
                });
            }
        }

        public bool IsValid()
        {
            if (this.Code.IsEmpty())
                throw new ArgumentException("É necessário enviar o código da loja");

            if (!Cnpj.IsNullOrWhiteSpace() && ((Cnpj.Length <= 11 && !Cnpj.IsValidCPF()) || (Cnpj.Length > 11 && !Cnpj.IsValidCNPJ())))
                throw new ArgumentException("CNPJ inválido");

            if (Name.IsNullOrWhiteSpace())
                throw new ArgumentException("Nome de loja inválido");

            if (Addresses.IsNull() || Addresses.Count == 0)
                throw new ArgumentException("Nenhum endereço fornecido para a loja");

            foreach (var address in Addresses)
            {
                if (address.ContactName.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem contato");

                if (address.ZipCode.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem CEP");

                if (address.Street.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem logradouro");

                if (address.Number.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem número");

                if (address.City.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem cidade");

                if (address.State.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem estado");

                if (address.PhoneNumber1.IsNullorEmpty())
                    throw new ArgumentException("Endereço sem telefone principal");
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