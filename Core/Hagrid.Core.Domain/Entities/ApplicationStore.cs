using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;
using DTO = Hagrid.Core.Domain.DTO;
using DTOSAS = Hagrid.Core.Domain.DTO.SaveAllSystem;

namespace Hagrid.Core.Domain.Entities
{
    public class ApplicationStore : IEntity, IStatus
    {
        public Guid Code { get; set; }

        public Guid ApplicationCode { get; set; }
        public Application Application { get; set; }

        public Guid StoreCode { get; set; }
        public Store Store { get; set; }

        public Guid? ConfClient { get; set; }
        public String ConfSecret { get; set; }

        public Guid? JSClient { get; set; }
        public String JSAllowedOrigins { get; set; }

        public ClientAuthType ClientAuthType { get; set; }

        public ICollection<AccountApplicationStore> AccountApplicationStoreCollection { get; set; }

        public bool Status { get; set; }

        public bool IsValidCall(string secret, string requestOrigin)
        {
            if (!secret.IsNullOrWhiteSpace())
            {
                this.ClientAuthType = Enums.ClientAuthType.Confidential;
                return IsValidConfCall(secret);
            }
            else if (!requestOrigin.IsNullOrWhiteSpace())
            {
                this.ClientAuthType = Enums.ClientAuthType.JavaScript;
                return IsValidJSCall(requestOrigin);
            }

            return false;
        }

        public bool IsValidJSCall(string requestOrigin)
        {
            if (JSAllowedOrigins.IsNullOrWhiteSpace()) return false;

            return JSAllowedOrigins.Split(',').Contains(requestOrigin);
        }

        public bool IsValidConfCall(string secret)
        {
            return ConfSecret == secret;
        }

        public Guid GetStoreCodeByAuthType(Guid? storeCode, bool addCustomer = false)
        {
            var _storeCode = Guid.Empty;

            //get store code
            switch (this.Application.AuthType)
            {
                case AuthType.Distributed:

                    _storeCode = this.Store.Code;

                    break;

                case AuthType.Unified:

                    if ((this.Store.IsMain && (!storeCode.HasValue || storeCode.IsEmpty()))
                        || addCustomer && (this.Application.MemberType == MemberType.Consumer || this.Application.MemberType == MemberType.Both))
                        _storeCode = this.Store.Code;
                    else if (!this.Store.IsMain && (!storeCode.HasValue || storeCode.IsEmpty()))
                        throw new ArgumentException("Ops! Você não tem permissão para executar essa ação");
                    else
                        _storeCode = storeCode.Value;

                    break;
            }

            return _storeCode;
        }
        public DTO.ApplicationStore Transfer()
        {
            return new DTO.ApplicationStore()
            {
                ConfidentialClient = this.ConfClient.Value,
                ConfidentialSecret = this.ConfSecret,
                JSClient = this.JSClient.Value,
                StoreCode = this.StoreCode,
                StoreName = this.Store.Name,
                ApplicationName = this.Application.Name,
                RefreshTokenLifeTimeInMinutes = this.Application.RefreshTokenLifeTimeInMinutes,
                MemberType = this.Application.MemberType.ToString(),
                AuthType = this.Application.AuthType.ToString(),
            };
        }

        public DTOSAS.Application TransferApp()
        {
            return new DTOSAS.Application()
            {
                ApplicationName = this.Application.Name,
                Secret = this.ConfSecret,
                Client = this.ConfClient.Value,
                JSClient = this.JSClient.Value,
                AllowedOrigins = this.JSAllowedOrigins,
                RefreshTokenLifeTimeInMinutes = this.Application.RefreshTokenLifeTimeInMinutes,
                MemberType = this.Application.MemberType.ToString(),
                AuthType = this.Application.AuthType.ToString()
            };
        }
    }
}
