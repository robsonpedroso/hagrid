using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ModelValidation;
using Hagrid.Core.Domain.Policies;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Hagrid.Core.Domain.Entities
{
    [Audit]
    public class Account : IEntity, IStatus, IDate, IIsValid, IResetPasswordTokenOwner
    {
        public Guid Code { get; set; }

        public string FacebookId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Document { get; set; }

        public ICollection<AccountApplicationStore> AccountApplicationStoreCollection { get; set; }

        public Customer Customer { get; set; }

        public int? QtyWrongsPassword { get; set; }

        public DateTime? LockedUp { get; set; }

        public bool? IsResetPasswordNeeded { get; set; }

        public bool Status { get; set; }

        [Audit(Type = AuditLogsType.Removed)]
        public bool Removed { get; set; }

        public string DataFingerprint { get; set; }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public PasswordEncryptionType OtherPasswordType { get; set; }

        public ICollection<Blacklist> BlacklistCollection { get; set; }

        public ICollection<AccountMetadata> Metadata { get; set; }

        public ICollection<StoreAccount> StoreAccounts { get; set; }

        [NotMapped]
        public int? CodeEmailTemplate { get; set; }

        public ICollection<AccountRole> AccountRoles { get; set; }

        public Account()
        {
            if (this.Code.IsEmpty())
            {
                Code = Guid.NewGuid();
                Status = true;
                Removed = false;
                SaveDate = DateTime.Now;
                UpdateDate = DateTime.Now;
                OtherPasswordType = PasswordEncryptionType.Default;
            }

            this.AccountApplicationStoreCollection = new List<AccountApplicationStore>();
        }

        public void SuccessfullyLogin(ILockedUpMemberPolicy lockedUpMemberPolicy, ILockMemberPolicy lockMemberPolicy)
        {
            if (lockedUpMemberPolicy != null && lockMemberPolicy != null)
            {
                if (this.QtyWrongsPassword.HasValue && this.QtyWrongsPassword > 0)
                {
                    this.QtyWrongsPassword = null;
                    this.LockedUp = null;
                }
            }
        }

        public void WrongLoginAttempt(ILockedUpMemberPolicy lockedUpMemberPolicy, ILockMemberPolicy lockMemberPolicy)
        {
            if (lockedUpMemberPolicy != null && lockMemberPolicy != null)
            {
                this.QtyWrongsPassword = this.QtyWrongsPassword.HasValue ? this.QtyWrongsPassword.Value + 1 : 1;

                if (!lockMemberPolicy.Validate(this))
                    LockMember();
            }
        }

        public void LockMember()
        {
            this.LockedUp = DateTime.Now.AddMinutes(Config.LockedTimeInMinutes.AsDouble());
        }

        public void SetPassword(string newPassword, IPasswordPolicy passwordPolicy)
        {
            if (passwordPolicy == null || passwordPolicy.Validate(this.Email, newPassword))
            {
                this.Password = newPassword.Encrypt();
            }
        }

        public object ValidatePassword(string password, IPasswordPolicy passwordPolicy)
        {
            try
            {
                if (passwordPolicy != null)
                    passwordPolicy.Validate(this.Email, password);

                return null;
            }
            catch (PasswordException ex)
            {
                return ex.Issues;
            }
        }

        public void ConnectApp(ApplicationStore applicationStore)
        {
            if (AccountApplicationStoreCollection.IsNull())
            {
                AccountApplicationStoreCollection = new List<AccountApplicationStore>() { new AccountApplicationStore(this.Code, applicationStore.Code) };
            }
            else if (!AccountApplicationStoreCollection.Any(x => x.ApplicationStoreCode == applicationStore.Code))
            {
                AccountApplicationStoreCollection.Add(new AccountApplicationStore(this.Code, applicationStore.Code));
            }
        }

        public void ConnectRole(Role role)
        {
            var accRole = new AccountRole(role, this.Code);

            if (AccountRoles.IsNull())
            {
                AccountRoles = new List<AccountRole>() { accRole };
            }
            else if (!AccountRoles.Any(x => x.RoleCode == role.Code))
            {
                AccountRoles.Add(accRole);
            }
        }

        public bool IsValid()
        {
            return IsValid(false);
        }

        public bool IsValid(bool simplifiedCustomer)
        {
            if (Login.IsNullOrWhiteSpace())
                throw new ArgumentException("Seu login está inválido");

            if (Password.IsNullOrWhiteSpace())
                throw new ArgumentException("Sua senha está inválida");

            if (Email.IsNullOrWhiteSpace())
                throw new ArgumentException("E-mail não informado");

            if (!Email.IsValidEmail())
                throw new ArgumentException("E-mail inválido");

            if (!simplifiedCustomer && !Document.IsNullOrWhiteSpace() && ((Document.Length <= 11 && !Document.IsValidCPF()) || (Document.Length > 11 && !Document.IsValidCNPJ())))
                throw new ArgumentException("Seu documento está inválido");

            return true;
        }

        public bool IsValidToUpdate(IEnumerable<Account> accounts)
        {
            IsValid();

            if (accounts.Any(a => a.Code != this.Code && a.Email == this.Email))
                throw new ArgumentException("Ops! Este e-mail já cadastrado");

            if (accounts.Any(a => a.Code != this.Code && a.Document == this.Document))
                throw new ArgumentException("Ops! Este documento já cadastrado");

            if (accounts.Any(a => a.Code != this.Code && a.Login == this.Login))
                throw new ArgumentException("Ops! Este login já cadastrado");

            return true;
        }

        public void Trim()
        {
            FacebookId = this.FacebookId.IsNullOrWhiteSpace() ? this.FacebookId : this.FacebookId.Trim();
            Login = this.Login.IsNullOrWhiteSpace() ? this.Login : this.Login.Trim();
            Email = this.Email.IsNullOrWhiteSpace() ? this.Email : this.Email.Trim();
            Document = this.Document.IsNullOrWhiteSpace() ? this.Document : this.Document.Trim();

            if (!Customer.IsNull())
                Customer.Trim();
        }

        public void Transfer(Account newAccount)
        {
            FacebookId = newAccount.FacebookId;
            Login = newAccount.Login;
            Email = newAccount.Email;
            Document = newAccount.Document;
            CodeEmailTemplate = !newAccount.CodeEmailTemplate.IsNull() ? newAccount.CodeEmailTemplate : null;
        }

        public void ChangeEmail(string emailNew)
        {
            if (!emailNew.Trim().IsValidEmail())
                throw new ArgumentException("E-mail inválido");

            if (this.Email.ToLower() == this.Login.ToLower())
                this.Login = emailNew.Trim();

            this.Email = emailNew;
            this.UpdateDate = DateTime.Now;

            if (!this.Customer.IsNull())
            {
                this.Customer.Email = emailNew;
                this.Customer.UpdateDate = DateTime.Now;
            }
        }

        public void ChangeDocument(string documentNew)
        {
            this.Document = documentNew;
            this.UpdateDate = DateTime.Now;

            if (!this.Customer.IsNull())
            {
                if (this.Customer is Person)
                    ((Person)this.Customer).Cpf = documentNew;
                else
                    ((Company)this.Customer).Cnpj = documentNew;

                this.Customer.UpdateDate = DateTime.Now;
            }
        }

        public void ChangeBirthdate(DateTime birthdateNew)
        {
            this.UpdateDate = DateTime.Now;

            if (!this.Customer.IsNull())
            {
                if (this.Customer is Person)
                    ((Person)this.Customer).BirthDate = birthdateNew;

                this.Customer.UpdateDate = DateTime.Now;
            }
        }

        public void SetRemoved()
        {
            this.Removed = true;
            this.Status = false;
            this.UpdateDate = DateTime.Now;

            if (!this.Customer.IsNull())
            {
                this.Customer.Removed = true;
                this.Customer.Status = false;
                this.Customer.UpdateDate = DateTime.Now;
            }
        }

        public static string GeneratePassword()
        {
            string newPassword = string.Empty;

            char[] charNewPassword = new char[] { 'F', 'D', '2', 'T', 'K', 'M', 'Q', 'X', 'P', 'Q', 'W', 'A', 'Z', 'C', 'J', 'E', '6', '4', '1', 'S', '8' };

            Random rand = new Random();

            for (int i = 0; i < 8; i++)
                newPassword = $"{newPassword}{charNewPassword[rand.Next(20)]}";

            return newPassword;
        }
    }
}