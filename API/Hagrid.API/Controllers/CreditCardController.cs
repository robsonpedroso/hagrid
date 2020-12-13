using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.DTO;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/member/credit-cards")]
    public class CreditCardController : BaseApiController
    {
        private readonly ICreditCardApplication creditCardApp;

        public CreditCardController(IRequestInfoService info, ICreditCardApplication creditCardApp)
            : base(info)
            => this.creditCardApp = creditCardApp;

        [HttpGet]
        [JwtAuthorization(Roles = "Member")]
        [Route("list/{accountCode?}")]
        public IHttpActionResult Get(Guid? accountCode = null)
        {
            try
            {
                if (base.Identity.clm["app:member_type"] == "MainAdmin" && !accountCode.IsNull())
                {
                    return Ok(creditCardApp.Get(accountCode.Value));
                }
                else
                {
                    return Ok(creditCardApp.Get(base.AccountCode));
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

        [HttpPost]
        [JwtAuthorization(Roles = "Member")]
        [Route("insert")]
        public IHttpActionResult Add(CreditCard creditCard)
        {
            try
            {
                return Ok(creditCardApp.Add(base.AccountCode, creditCard));
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
        [Route("{cardCode}")]
        public IHttpActionResult Remove(string cardCode)
        {
            try
            {
                creditCardApp.Remove(base.AccountCode, cardCode);
                
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
    }
}
