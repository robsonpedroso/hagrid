using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/store-credit-card")]
    public class StoreCreditCardController : BaseApiController
    {
        private readonly IStoreCreditCardApplication creditCardApp;

        public StoreCreditCardController(IRequestInfoService info, IStoreCreditCardApplication creditCardApp)
            : base(info)
            => this.creditCardApp = creditCardApp;

        [HttpGet]
        [Route("secret-phrase/{phrase}")]
        [JwtAuthorization(Roles = "Application")]
        public IHttpActionResult GetSecretPhrase(string phrase)
        {
            try
            {
                return Ok(creditCardApp.GetSecretPhrase(phrase));
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
        [Route("")]
        [JwtAuthorization(Roles = "Application")]
        public IHttpActionResult Save(DTO.StoreCreditCard credicard)
        {
            try
            {
                creditCardApp.Save(credicard);

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