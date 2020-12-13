using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ExceptionObjects;
using Hagrid.Core.Domain.Policies;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/member")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountApplication accountApp;
        private readonly IStoreApplication storeApp;
        private readonly IResetPasswordTokenApplication resetPasswordTokenApp;
        private readonly IResetSMSTokenApplication resetSMSTokenApplication;
        private readonly IApplicationStoreApplication appStoreApp;
        private readonly ITransferTokenApplication transferTokenApp;

        public AccountController(
            IRequestInfoService info,
            IStoreApplication storeApp,
            IApplicationStoreApplication appStoreApp,
            ITransferTokenApplication transferTokenApp,
            IAccountResetPasswordTokenApplication resetPasswordTokenApp,
            IResetSMSTokenApplication resetSMSTokenApplication,
            IAccountApplication accountApp)
            : base(info)
        {
            this.accountApp = accountApp;
            this.storeApp = storeApp;
            this.appStoreApp = appStoreApp;
            this.transferTokenApp = transferTokenApp;
            this.resetPasswordTokenApp = resetPasswordTokenApp;
            this.resetSMSTokenApplication = resetSMSTokenApplication;
        }

        #region "  Save Or Update  "

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("")]
        public IHttpActionResult AddAccount(DTO.Account account)
        {
            try
            {
                base.SetRequestInfo();
                return Ok(accountApp.Save(account, base.ClientId, base.AccountCode));
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (PasswordException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("simplified")]
        public IHttpActionResult AddSimplifiedAccount(DTO.Account customerSimplified)
        {
            try
            {
                base.SetRequestInfo();
                return Ok(accountApp.SaveCustomerSimplified(customerSimplified, base.ClientId));
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (PasswordException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Member,Application", AppProperties = "member_type=MainAdmin")]
        [Route("save-account-admin")]
        public IHttpActionResult AddAccountByAdmin(DTO.Account account)
        {
            try
            {
                base.SetRequestInfo();

                var _account = accountApp.Save(account, base.ClientId, Guid.Empty, false, true);

                if (!_account.IsNull() && !_account.Password.IsNullOrWhiteSpace())
                    _account.Password = null;

                return Ok(_account);
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (PasswordException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [JwtAuthorization(Roles = "Member")]
        [Route("")]
        public IHttpActionResult UpdateAccount(DTO.Account account)
        {
            try
            {
                base.SetRequestInfo();

                return Ok(accountApp.Save(account, base.ClientId, base.AccountCode));
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (PasswordException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=Merchant, MainAdmin")]
        [Route("info")]
        public IHttpActionResult ChangeAccountInfo(DTO.AccountInput input)
        {
            try
            {
                if (!base.IsMainAdmin)
                    throw new ArgumentException("Não tem permissão para executar esse método");

                base.SetRequestInfo();

                accountApp.ChangeAccountInfo(input, base.AccountCode);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Unlock  "

        [HttpGet]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("{code}/unlock-user")]
        public IHttpActionResult UnlockUser(Guid code)
        {
            try
            {
                return Ok(accountApp.UnlockUser(code, base.AccountCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #endregion

        #region "  Remove  "

        [HttpDelete]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("{code}")]
        public IHttpActionResult Remove(Guid code)
        {
            try
            {
                base.SetRequestInfo();

                accountApp.Remove(code, base.AccountCode);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Get Member  "

        [HttpGet]
        [JwtAuthorization(Roles = "Member")]
        [Route("")]
        public IHttpActionResult GetMember()
        {
            try
            {
                return Ok(accountApp.Get(base.AccountCode, base.ClientId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Application")]
        [Route("{code}")]
        public IHttpActionResult GetMember(Guid code)
        {
            try
            {
                return Ok(accountApp.Get(code, base.ClientId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Application", AppProperties = "auth_type=Confidential")]
        [Route("get-by-document/{document}")]
        public IHttpActionResult GetMember(string document)
        {
            try
            {
                return Ok(accountApp.Get(document, base.ClientId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application", AppProperties = "auth_type=Confidential")]
        [Route("get-by-email")]
        public IHttpActionResult GetMemberByEmail(AccountInput param)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(param.email))
                {
                    return Ok(accountApp.GetEmail(param.email, base.ClientId));
                }
                else
                {
                    throw new ArgumentException("O parâmetro email não pode ser nulo");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("{code}/detail")]
        public IHttpActionResult GetDetail(Guid code)
        {
            try
            {
                return Ok(accountApp.GetDetail(code, base.AccountCode, base.AccountEmail, true, true));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("search")]
        public IHttpActionResult SearchAccounts(DTO.SearchFilterAccount filter)
        {
            try
            {
                return Ok(accountApp.Search(filter));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Exists  "

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("exists")]
        public IHttpActionResult IsMemberExists(DTO.AccountInput param)
        {
            try
            {
                return Ok((object)accountApp.IsMemberExists(param));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("exists-by-type")]
        public IHttpActionResult IsMemberExistsByType(DTO.AccountInput param)
        {
            try
            {
                return Ok((object)accountApp.IsMemberExists(param));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Password  "

        [HttpPost]
        [JwtAuthorization(Roles = "ChangePassword, ResetPassword, Member")]
        [Route("password-validate")]
        public IHttpActionResult ValidatePassword(DTO.AccountInput param)
        {
            try
            {
                return Ok(accountApp.ValidatePassword(base.AccountCode, param.Password));
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (PasswordException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Reset Password  "

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("password-reset/email/test")]
        public IHttpActionResult SendResetPasswordEmail(AccountInput param)
        {
            return SendResetPasswordEmail(param, true);
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("password-reset/email/with-check")]
        public IHttpActionResult SendResetPasswordEmailWithCheck(AccountInput param)
        {
            return SendResetPasswordEmail(param, throwError: true);
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("password-reset/email")]
        public IHttpActionResult SendResetPasswordEmail(AccountInput param, bool test = false, bool throwError = false)
        {
            try
            {
                var token = resetPasswordTokenApp.GenerateResetPasswordToken(param.email, base.ClientId, param.url_back, param.email_template_code);
#if DEBUG
                if (test)
                    return Ok(new { reset_password_token = token });
#endif
                if (throwError && token.IsNullOrWhiteSpace())
                    return NotFound();

                return Ok();
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (AccountNotFoundException)
            {
                if (throwError)
                    return NotFound();

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "ResetPassword")]
        [Route("password-reset")]
        public IHttpActionResult ResetPassword(AccountInput param)
        {

            var identity = ActionContext.ActionArguments["identity"] as JwtToken;

            if (param != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(param.password))
                    {
                        accountApp.ResetMemberPassword(base.AccountCode, identity.name, param.password, base.ClientId);
                        return Ok();
                    }
                    else
                    {
                        throw new ArgumentException("o parâmetro password não pode ser nulo");
                    }
                }
                catch (LockedUpMemberException ex)
                {
                    return BadRequest(new ArgumentException(ex.Message));
                }
                catch (PasswordException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("send-password-email")]
        public IHttpActionResult SendPasswordEmail(DTO.AccountInput input)
        {
            try
            {
                DTO.ApplicationStore appStore = null;
                Guid? confId = null;
                if (base.IsMainAdmin)
                {
                    appStore = appStoreApp.GetApplicationStoreByStoreTypeMain("Hagrid-UI-Login");
                    if (!appStore.IsNull())
                        confId = appStore.ConfidentialClient;
                }
                else
                {
                    confId = base.ClientId;
                }

                if (!confId.HasValue)
                    throw new ArgumentException("Aplicação não encontrada");

                resetPasswordTokenApp.GenerateResetPasswordToken(input.Email, confId.Value);

                return Ok();
            }
            catch (LockedUpMemberException ex)
            {
                return BadRequest(new ArgumentException(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Reset Password SMS   "

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("password-reset/sms")]
        public IHttpActionResult GetTokenSMS(AccountInput param)
        {
            try
            {
                var account = new DTO.AccountInput()
                {
                    Email = param.email,
                    Document = param.document,
                    UrlBack = param.url_back
                };

                var token = accountApp.GetTokenSMS(account, base.ClientId);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "ResetAccountSMS")]
        [Route("reset-account")]
        public IHttpActionResult ResetAccountSMS(DTO.AccountInput account)
        {
            try
            {
                base.SetRequestInfo();

                accountApp.ResetAccount(account, base.AccountCode);
                return Ok();
            }
            catch (LockedUpMemberException)
            {
                return BadRequest("locked_member");
            }
            catch (PasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("sms/sended/{customerCode}")]
        public IHttpActionResult GetSendedSms(Guid customerCode)
        {
            try
            {
                return Ok(resetSMSTokenApplication.GetSendedSms(customerCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Change Password  "

        [HttpPost]
        [JwtAuthorization(Roles = "Member")]
        [Route("password-change/token")]
        public IHttpActionResult ChangePasswordToken(AccountInput param)
        {
            try
            {
                string urlBack = string.Empty;
                bool showMessage = false;

                if (param != null)
                {
                    urlBack = param.url_back;
                    showMessage = param.show_change_password_message;
                }

                var changePasswordToken = resetPasswordTokenApp.CreateChangePasswordToken(base.AccountCode, base.ClientId, urlBack, showMessage);

                return Ok(new
                {
                    url = changePasswordToken.ToString(),
                    token = changePasswordToken.Token
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "ChangePassword")]
        [Route("password-change")]
        public IHttpActionResult ChangePassword(AccountInput param)
        {
            if (param != null)
            {
                if (!string.IsNullOrWhiteSpace(param.password))
                {
                    if (!string.IsNullOrWhiteSpace(param.password_new))
                    {
                        try
                        {
                            accountApp.ChangeMemberPassword(base.AccountCode, param.password, param.password_new, base.ClientId);
                            return Ok();
                        }
                        catch (LockedUpMemberException)
                        {
                            return BadRequest("locked_member");
                        }
                        catch (PasswordException ex)
                        {
                            return BadRequest(ex.Message);
                        }
                        catch (ArgumentException ex)
                        {
                            return BadRequest(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            return InternalServerError(ex);
                        }
                    }
                    else
                    {
                        return BadRequest("passwordNew");
                    }
                }
                else
                {
                    return BadRequest("password");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region "  Transfer Token  "

        [HttpGet]
        [JwtAuthorization(Roles = "Member")]
        [Route("get-transfer-token")]
        public IHttpActionResult GetTransferToken()
        {
            var identity = ActionContext.ActionArguments["identity"] as JwtToken;

            try
            {
                var transferToken = new TransferToken(base.AccountCode, identity.name, base.ClientId);
                transferTokenApp.Save(transferToken);

                return Ok(new { transfer_token = transferToken.Code });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Permissions  "

        [HttpPost]
        [JwtAuthorization(Roles = "Member,Application", AppProperties = "member_type=Merchant, MainAdmin")]
        [Route("permission/{type}")]
        public IHttpActionResult UpdatePermission([FromUri]PermissionType type, DTO.AccountPermission accountPermission)
        {
            try
            {
                accountApp.UpdatePermission(accountPermission, type, base.ClientId, base.AccountCode);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=Merchant, MainAdmin")]
        [Route("get-stores")]
        public IHttpActionResult GetStores()
        {
            try
            {
                return Ok(storeApp.GetByMember(base.AccountCode, base.ClientId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Application")]
        [Route("get-application-stores/{accountCode}")]
        public IHttpActionResult GetApplicationStores(Guid accountCode)
        {
            try
            {
                return Ok(appStoreApp.GetByAccount(accountCode, base.ClientId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region "  Account  Permission   "

        [HttpPost]
        [JwtAuthorization(Roles = "Member")]
        [Route("{accountCode}/roles/{roleCode}")]
        public IHttpActionResult LinkAccountRole(Guid accountCode, Guid roleCode)
        {
            try
            {
                accountApp.LinkAccountRole(accountCode, roleCode, base.IsMainAdmin, base.ClientId);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Member")]
        [Route("roles")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(accountApp.GetRole(base.AccountCode, base.ClientId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [JwtAuthorization(Roles = "Member")]
        [Route("{accountCode}/roles/{roleCode}")]
        public IHttpActionResult UnLinkAccountRole(Guid accountCode, Guid roleCode)
        {
            try
            {
                accountApp.UnLinkAccountRole(accountCode, roleCode, base.IsMainAdmin, base.ClientId);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #endregion

        [HttpGet]
        [Route("account/ping")]
        public new IHttpActionResult Ping() => base.Ping();
    }

    public class AccountInput
    {
        public string email { get; set; }
        public string cpf { get; set; }
        public string cnpj { get; set; }
        public string password { get; set; }
        public string password_new { get; set; }
        public string url_back { get; set; }
        public bool show_change_password_message { get; set; }
        public string document { get; set; }
        public int email_template_code { get; set; }
    }
}
