using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/member")]
    public class AccountAddressController : BaseApiController
    {
        private readonly IAccountApplication accountApp;

        public AccountAddressController(IRequestInfoService info, IAccountApplication accountApp) 
            : base(info)
            => this.accountApp = accountApp;

        [HttpDelete]
        [JwtAuthorization(Roles = "Member")]
        [Route("{accountCode}/adresses/{addressCustomerCode}")]
        public IHttpActionResult Remove(Guid accountCode, Guid addressCustomerCode)
        {
            try
            {
                return Delete(accountCode, addressCustomerCode);
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
        [Route("{accountCode}/addresses/{addressCustomerCode}")]
        public IHttpActionResult Delete(Guid accountCode, Guid addressCustomerCode)
        {
            try
            {
                var accCode = base.IsMainAdmin ? accountCode : base.AccountCode;

                accountApp.RemoveAddress(accCode, addressCustomerCode);

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
        [Route("{accountCode}/addresses")]
        public IHttpActionResult GetAdresses(Guid accountCode)
        {
            try
            {
                var accCode = base.IsMainAdmin ? accountCode : base.AccountCode;

                return Ok(accountApp.GetAdresses(accCode));
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
        [Route("{accountCode}/addresses/{code}")]
        public IHttpActionResult GetAdressByCode(Guid accountCode, Guid code)
        {
            try
            {
                var accCode = base.IsMainAdmin ? accountCode : base.AccountCode;

                return Ok(accountApp.GetByCode(accCode, code));
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
        [Route("{accountCode}/adresses")]
        public IHttpActionResult AddOrUpdateAddress(DTO.Address adresses, Guid accountCode)
        {
            return Save(adresses, accountCode);
        }


        [HttpPost]
        [JwtAuthorization(Roles = "Member")]
        [Route("{accountCode}/addresses")]
        public IHttpActionResult Save(DTO.Address adresses, Guid accountCode)
        {
            try
            {
                var accCode = base.IsMainAdmin ? accountCode : base.AccountCode;

                var _address = accountApp.Save(adresses, accCode);

                return Ok(new
                {
                    address_customer_code = _address.AddressCustomerCode
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

        [HttpGet]
        [Route("addresses/ping")]
        public new IHttpActionResult Ping() => base.Ping();
    }
}