using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/permissions")]
    public class PermissionsController : BaseApiController
    {
        private readonly IPermissionApplication permissionApp;

        public PermissionsController(IRequestInfoService info, IPermissionApplication permissionApp)
            : base(info)
            => this.permissionApp = permissionApp;

        [HttpPost]
        [JwtAuthorization(Roles = "Member")]
        [Route("")]
        public IHttpActionResult Save(DTO.Permission permission)
        {
            try
            {
                return Ok(permissionApp.Save(base.IsMainAdmin, base.ClientId, permission));
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
        public IHttpActionResult Get(Guid? role_code = null, Guid? application_code = null, Guid? resource_code = null, string resource_name = null, int? skip = null, int? take = null)
        {
            try
            {
                return Ok(permissionApp.Search(base.IsMainAdmin, base.ClientId, role_code, application_code, resource_code, resource_name, skip, take));
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
                return Ok(permissionApp.Get(base.IsMainAdmin, base.ClientId, code));
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
                permissionApp.Delete(code);
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