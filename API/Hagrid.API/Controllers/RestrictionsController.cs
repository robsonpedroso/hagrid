using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/roles/{roleCode:guid}/restrictions")]
    public class RestrictionsController : BaseApiController
    {
        private readonly IRestrictionApplication restrictionApplication;

        public RestrictionsController(IRequestInfoService info, IRestrictionApplication restrictionApplication)
            : base(info)
            => this.restrictionApplication = restrictionApplication;

        [HttpPost, Route(""), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Save(Guid roleCode, DTO.Restriction restriction)
        {
            try
            {
                if (restriction.IsNull())
                    throw new ArgumentException("Os dados da restrição enviado é nulo ou está em um formato inválido.");

                if (!base.IsMainAdmin || restriction.Role.IsNull()  || restriction.Role.IsNull() || restriction.Role.Code.IsEmpty())
                    restriction.Role = new DTO.Role() { Code = roleCode };

                return Ok(restrictionApplication.Save(base.IsMainAdmin, base.ClientId, restriction));
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
        public IHttpActionResult Get(Guid roleCode, DTO.SearchFilterRestriction filterRestriction)
        {
            try
            {
                if (filterRestriction.IsNull())
                    throw new ArgumentException("Os dados do filtro enviado é nulo ou está em um formato inválido.");

                if (!base.IsMainAdmin || filterRestriction.RoleCode.IsNull() || filterRestriction.RoleCode.IsEmpty())
                    filterRestriction.RoleCode = roleCode;

                return Ok(restrictionApplication.Search(base.IsMainAdmin, base.ClientId, filterRestriction));
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

        [HttpGet, Route("{code}"), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Get(Guid roleCode, Guid code)
        {
            try
            {
                return Ok(restrictionApplication.Get(base.IsMainAdmin, base.ClientId, roleCode, code));
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

        [HttpDelete, Route("{code}"), JwtAuthorization(Roles = "Member")]
        public IHttpActionResult Delete(Guid roleCode, Guid code)
        {
            try
            {
                restrictionApplication.Delete(roleCode, code);
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