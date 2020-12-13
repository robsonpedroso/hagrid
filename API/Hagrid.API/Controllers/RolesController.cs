using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/roles")]
    public class RolesController : BaseApiController
    {
        private readonly IRoleApplication roleApp;

        public RolesController(IRequestInfoService info, IRoleApplication roleApp)
            : base(info)
            => this.roleApp = roleApp;

        [HttpPost]
        [JwtAuthorization(Roles = "Member")]
        [Route("")]
        public IHttpActionResult Save(DTO.Role role)
        {
            try
            {
                return Ok(roleApp.Save(role));
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
        [Route("{code}")]
        public IHttpActionResult Get(Guid code)
        {
            try
            {
                return Ok(roleApp.Get(base.StoreCode, base.IsMainAdmin, code, false));
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
        [Route("{code}/detail")]
        public IHttpActionResult GetDetail(Guid code)
        {
            try
            {
                return Ok(roleApp.Get(base.StoreCode, base.IsMainAdmin, code, true));
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
        [Route("")]
        public IHttpActionResult Get(Guid store_code, string name = "", int? skip = null, int? take = null)
        {
            try
            {
                var _storeCode = base.IsMainAdmin ? store_code : base.StoreCode;

                return Ok(roleApp.Search(_storeCode, name, skip, take));
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
        [Route("{code}")]
        public IHttpActionResult Delete(Guid code)
        {
            try
            {
                roleApp.Delete(code);

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