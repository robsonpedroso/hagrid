using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.ValueObjects;
using System;
using System.Web.Http;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/auth")]
    public class AuthAdminController : BaseApiController
    {
        private readonly IAccountApplication accountApplication;

        public AuthAdminController(IRequestInfoService info, IAccountApplication accountApplication)
            : base(info)
            => this.accountApplication = accountApplication;

        [HttpGet]
        [Route("{transferToken}")]
        public IHttpActionResult TransferToken(string transferToken)
        {
            try
            {
                var account = accountApplication.AccountsAdminTransferToken(transferToken);

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
    }
}