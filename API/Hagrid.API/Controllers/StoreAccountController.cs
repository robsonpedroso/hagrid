using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/store/{storeCode}/accounts")]
    public class StoreAccountController : BaseApiController
    {
        private readonly IStoreAccountApplication storeAccountApp;

        public StoreAccountController(IRequestInfoService info, IStoreAccountApplication storeAccountApp)
            : base(info)
            => this.storeAccountApp = storeAccountApp;

        [HttpPost, Route(""), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Save(Guid storeCode, DTO.StoreAccount storeAccount)
        {
            try
            {
                if (storeAccount.IsNull())
                    throw new ArgumentException("Os dados do vínculo enviado é nulo ou está em um formato inválido.");

                if (!base.IsMainAdmin && base.StoreCode != storeCode)
                    throw new ArgumentException("Loja informada diferente da loja do token.");

                storeAccount.SetStore(storeCode);
                return Ok(storeAccountApp.Save(storeAccount));
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

        [HttpGet, Route("{accountCode}"), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Get(Guid storeCode, Guid accountCode)
        {
            try
            {
                if (!base.IsMainAdmin && base.StoreCode != storeCode)
                    throw new ArgumentException("Loja informada diferente da loja do token.");

                return Ok(storeAccountApp.Get(storeCode, accountCode));
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

        [HttpDelete, Route("{accountCode}"), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Delete(Guid storeCode, Guid accountCode)
        {
            try
            {
                if (!base.IsMainAdmin && base.StoreCode != storeCode)
                    throw new ArgumentException("Loja informada diferente da loja do token.");

                storeAccountApp.Delete(storeCode, accountCode);
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

        [HttpPost, Route("search"), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Search(Guid storeCode, DTO.SearchFilterStoreAccount filter)
        {
            try
            {
                if (filter.IsNull())
                    throw new ArgumentException("Os dados do filtro enviado é nulo ou está em um formato inválido.");

                if (!base.IsMainAdmin && base.StoreCode != storeCode)
                    throw new ArgumentException("Loja informada diferente da loja do token.");

                filter.StoreCode = storeCode;

                return Ok(storeAccountApp.Search(filter));
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
