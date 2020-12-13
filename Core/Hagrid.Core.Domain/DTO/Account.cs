using Newtonsoft.Json;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    public class Account
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("facebook_id")]
        public string FacebookId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }

        [JsonProperty("applications")]
        public string[] Applications { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }

        [JsonProperty("removed")]
        public bool? Removed { get; set; }

        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("save_date")]
        public DateTime? SaveDate { get; set; }

        [JsonProperty("update_date")]
        public DateTime? UpdateDate { get; set; }

        [JsonProperty("data_fingerprint")]
        public string DataFingerprint { get; set; }

        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("token")]
        public AccountToken Token { get; set; }

        [JsonProperty("qty_wrongs_password")]
        public int? QtyWrongsPassword { get; set; }

        [JsonProperty("locked_up")]
        public DateTime? LockedUp { get; set; }

        [JsonProperty("applications_store")]
        public List<DTO.AccountApplicationStore> ApplicationsStore { get; set; }

        [JsonProperty("blacklist")]
        public List<DTO.Blacklist> Blacklist { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        [JsonProperty("metadata_fields")]
        public object MetadataFields { get; set; }

        [JsonProperty("code_email_template")]
        public int? CodeEmailTemplate { get; set; }

        [JsonProperty("roles")]
        public List<Role> Roles { get; set; }

        public Account() { }

        public Account(DO.Account account)
        {
            Code = account.Code;
            FacebookId = account.FacebookId;
            Login = account?.Login?.Trim();
            Email = account?.Email?.Trim();
            Password = account.Password;
            Document = account?.Document?.ClearStrings();
            Status = account.Status;
            SaveDate = account.SaveDate;
            UpdateDate = account.UpdateDate;
            DataFingerprint = account.DataFingerprint;

            QtyWrongsPassword = account.QtyWrongsPassword;
            LockedUp = account.LockedUp;
            ApplicationsStore = null;

            if (!account.AccountRoles.IsNull() && account.AccountRoles.Count > 0 && !account.AccountRoles.Any(r => r.Role.Permissions.IsNull()))
                Roles = account.AccountRoles.Where(r => r.Status && r.Role.Status && r.Role.Permissions.Any(p => p.Status)).Select(r => new Role()
                {
                    Code = r.Role.Code,
                    Name = r.Role.Name,
                    Status = r.Role.Status,
                    Type = r.Role.Type,
                    TypeCode = r.Role.Type.AsInt(),
                    TypeDescription = r.Role.Type.GetDescription(),
                    Store = new Store()
                    {
                        Code = r.Role.Store.Code,
                        Name = r.Role.Store.Name
                    }
                }).ToList();

            if (!account.Metadata.IsNull())
            {
                var metadata = new Dictionary<string, object>();

                account.Metadata.ForEach(m => metadata.Add(m.Field.JsonId, m.Value));

                this.Metadata = metadata;

                this.MetadataFields = account.Metadata.OrderBy(m => m.Field.SaveDate).Select(m => new DTO.MetadataField(m.Field, m)).ToList();
            }

            if (!account.Customer.IsNull() && account.Customer is DO.Customer)
            {
                switch (account.Customer.GetType().Name)
                {
                    case "Person":
                        Customer = new Person((DO.Person)account.Customer);
                        break;
                    case "Company":
                        Customer = new Company((DO.Company)account.Customer);
                        break;
                    default:
                        Customer = new Customer(account.Customer);
                        break;
                }
            }
        }

        public Account(DO.Account account, ICollection<DO.ApplicationStore> applicationStores)
            : this(account)
        {
            if (!applicationStores.IsNull())
            {
                Applications = null;
                ApplicationsStore = applicationStores.Select(app => new DTO.AccountApplicationStore(app)).OrderBy(a => a.Name).ToList();
            }
        }

        public Account(DO.Account account, ICollection<DO.ApplicationStore> applicationStores, bool blacklist = false)
            : this(account, applicationStores)
        {
            if (blacklist)
            {
                if (!account.BlacklistCollection.IsNull())
                    Blacklist = account.BlacklistCollection.Where(x => x.Blocked).Select(x => new DTO.Blacklist(x)).ToList();
            }
        }

        public virtual DO.Account Transfer()
        {
            var account = new DO.Account();

            account.FacebookId = this.FacebookId.IsNullOrWhiteSpace() ? this.FacebookId : this.FacebookId.Trim();
            account.Password = this.Password;
            account.Login = this.Login.IsNullOrWhiteSpace() ? this.Login : this.Login.Trim();
            account.Email = this.Email.IsNullOrWhiteSpace() ? this.Email : this.Email.Trim();
            account.Document = this.Document.IsNullOrWhiteSpace() ? this.Document : this.Document.Trim();
            account.DataFingerprint = this.DataFingerprint.IsNullOrWhiteSpace() ? this.DataFingerprint : this.DataFingerprint.Trim();

            if (!this.Customer.IsNull())
                account.Customer = this.Customer.Transfer(this);

            return account;
        }

        public DTO.Account TransferSimplified()
        {
            var accountSimplified = new DTO.Account();

            accountSimplified.Code = this.Code;
            accountSimplified.Email = this.Email;
            accountSimplified.Login = this.Login;
            accountSimplified.Password = string.Empty;
            accountSimplified.Document = string.Empty;
            accountSimplified.DataFingerprint = this.DataFingerprint;
            accountSimplified.Customer = new Customer()
            {
                Addresses = new List<Address>(),
                Type = this.Customer.Type,
                OriginStore = this.Customer.OriginStore,
                Name = this.Customer.Name,
                SaveDate = this.Customer.SaveDate,
                UpdateDate = this.Customer.UpdateDate,
                Status = this.Customer.Status
            };


            if (this.Customer is DTO.Person)
                accountSimplified.Name = ((DTO.Person)this.Customer).FirstName;
            else
                accountSimplified.Name = ((DTO.Company)this.Customer).CompanyName;

            accountSimplified.Metadata = this.Metadata;
            accountSimplified.Status = this.Status;
            accountSimplified.Removed = this.Removed;

            return accountSimplified;
        }

        public DO.Account Transfer(List<DO.ApplicationStore> applicationStores)
        {
            var account = new DO.Account()
            {
                FacebookId = this.FacebookId,
                Password = this.Password,
                Login = this.Login,
                Email = this.Email,
                AccountApplicationStoreCollection = applicationStores.Select(a => new DO.AccountApplicationStore(this.Code, a.Code)).ToList(),
                Document = this.Document.ClearStrings(),
                DataFingerprint = this.DataFingerprint,
                CodeEmailTemplate = this.CodeEmailTemplate
            };

            if (!this.Customer.IsNull())
            {
                account.Customer = this.Customer.Transfer(this);
            }
            else if (this.Document.IsValidCNPJ())
            {
                account.Customer = new DO.Company()
                {
                    Cnpj = this.Document.ClearStrings(),
                    Email = this.Email,
                    Name = !this.Name.IsNullOrWhiteSpace() ? this.Name : this.Email,
                    Addresses = new List<AddressCustomer>()
                };
            }
            else
            {
                account.Customer = new DO.Person()
                {
                    FirstName = !this.Name.IsNullOrWhiteSpace() ? this.Name : this.Email,
                    Email = this.Email,
                    Gender = string.Empty,
                    Cpf = this.Document.IsValidCPF() ? this.Document.ClearStrings() : string.Empty,
                    Addresses = new List<AddressCustomer>()
                };
            }

            return account;
        }

        public ICollection<DO.AccountMetadata> GetMetadata(Guid accountCode)
        {
            var metadata = this.Metadata.ToJsonString().JsonTo<Dictionary<string, string>>();

            if (!this.Metadata.IsNull())
                return metadata.Select(m => new DO.AccountMetadata(accountCode, new MetadataField { JsonId = m.Key, Value = m.Value })).ToList();
            else
                return new List<DO.AccountMetadata>();
        }
    }
}