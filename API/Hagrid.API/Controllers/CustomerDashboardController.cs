using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.ValueObjects;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/customer-dashboard")]
    public class CustomerDashboardController : BaseApiController
    {
        private readonly IAccountApplication accountApplication;

        public CustomerDashboardController(IRequestInfoService info, IAccountApplication accountApplication)
            : base(info)
            => this.accountApplication = accountApplication;

        [HttpGet]
        [Route("auth/{transferToken}")]
        public IHttpActionResult TransferToken(string transferToken)
        {
            try
            {
                var account = accountApplication.CustomerDashboardTransferToken(transferToken);

                return Ok(account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (ForbiddenException)
            {
                return Forbidden();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("change-password/token")]
        public IHttpActionResult ChangePasswordToken(DTO.ChangePasswordToken changePasswordToken)
        {
            try
            {
                var _changePasswordToken = accountApplication.CustomerDashboardChangePasswordToken(changePasswordToken.Token);

                return Ok(_changePasswordToken);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (ForbiddenException)
            {
                return Forbidden();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("change-password")]
        public IHttpActionResult ChangePassword(DTO.ChangePassword changePassword)
        {
            try
            {
                var _changePasswordToken = accountApplication.CustomerDashboardChangePassword(changePassword);

                return Ok(_changePasswordToken);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (ForbiddenException)
            {
                return Forbidden();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}