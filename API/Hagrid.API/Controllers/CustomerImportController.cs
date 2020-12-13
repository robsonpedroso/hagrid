using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Policies;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("member-import")]
    public class CustomerImportController : BaseApiController
    {
        private readonly ICustomerImportApplication customerImportApp;
        private readonly IResetPasswordTokenApplication resetPasswordTokenApp;

        public CustomerImportController(IRequestInfoService info, 
            ICustomerImportApplication customerImportApp, 
            ICustomerImportResetPasswordTokenApplication resetPasswordTokenApp)
            : base(info)
        {
            this.customerImportApp = customerImportApp;
            this.resetPasswordTokenApp = resetPasswordTokenApp;
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("exists")]
        public IHttpActionResult Exists(CustomerImportinput input)
        {
            try
            {
                return Ok((object)customerImportApp.Exists(input.username, input.store_code));
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
        [Route("password-reset/email/test")]
        public IHttpActionResult SendResetPasswordEmail(CustomerImportinput input)
        {
            return SendResetPasswordEmail(input, true);
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("password-reset/email/with-check")]
        public IHttpActionResult SendResetPasswordEmailWithCheck(CustomerImportinput param)
        {
            return SendResetPasswordEmail(param, throwError: true);
        }

        [HttpPost]
        [JwtAuthorization(Roles = "Application")]
        [Route("password-reset/email")]
        public IHttpActionResult SendResetPasswordEmail(CustomerImportinput input, bool test = false, bool throwError = false)
        {
            var identity = ActionContext.ActionArguments["identity"] as JwtToken;

            if (input != null)
            {
                try
                {
                    if (input.username.IsNullOrWhiteSpace())
                    {
                        throw new ArgumentException("o parâmetro email não pode nulo");
                    }
                    else if (input.store_code.IsEmpty())
                    {
                        throw new ArgumentException("o parâmetro storeCode não pode nulo");


                    }
                    else
                    {
                        var token = resetPasswordTokenApp.GenerateResetPasswordToken(input.username, input.store_code, input.url_back);
#if DEBUG
                        if (test)
                            return Ok(new { reset_password_token = token });
#endif
                        if (throwError && token.IsNullOrWhiteSpace())
                            return NotFound();

                        return Ok();
                    }
                }
                catch (LockedUpMemberException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [JwtAuthorization(Roles = "ResetPassword")]
        [Route("password-reset")]
        public IHttpActionResult ResetPassword(CustomerImportinput param)
        {
            var identity = ActionContext.ActionArguments["identity"] as JwtToken;

            if (param != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(param.password))
                    {
                        customerImportApp.CreatePassword(base.AccountCode, identity.name, param.password, base.ClientId);
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
        [JwtAuthorization(Roles = "ChangePassword, ResetPassword, Member")]
        [Route("password-validate")]
        public IHttpActionResult ValidatePassword(CustomerValidatePasswordinput param)
        {
            try
            {
                return Ok(customerImportApp.ValidatePassword(base.AccountCode, param.password));
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
        [Route("remove")]
        public IHttpActionResult Remove(CustomerImportinput input)
        {
            if (input != null)
            {
                try
                {
                    customerImportApp.RemoveMember(input.email, input.document);

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
            else
            {
                return BadRequest();
            }
        }
    }

    public class CustomerImportinput
    {
        public string username { get; set; }
        public Guid store_code { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string document { get; set; }
        public string url_back { get; set; }
    }

    public class CustomerValidatePasswordinput
    {
        public string password { get; set; }
    }
}
