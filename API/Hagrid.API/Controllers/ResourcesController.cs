using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/applications/resources")]
    public class ResourcesController : BaseApiController
    {
        private readonly IResourceApplication resourceApp;

        public ResourcesController(IRequestInfoService info, IResourceApplication resourceApp)
            : base(info)
            => this.resourceApp = resourceApp;

        [HttpGet]
        [JwtAuthorization(Roles = "Member")]
        [Route("")]
        public IHttpActionResult GetResources(string name = null, Guid? application_code = null, int? skip = null, int? take = null)
        {
            try
            {
                var result = resourceApp.Search(base.IsMainAdmin, base.ClientId, name, application_code, skip, take);

                return Ok(result);
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
        public IHttpActionResult GetResources(Guid code)
        {
            try
            {
                var result = resourceApp.GetResource(base.IsMainAdmin, base.ClientId, code);
                return Ok(result);
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
        [Route("")]
        public IHttpActionResult Save(DTO.Resource resource)
        {
            try
            {
                var result = resourceApp.Save(base.IsMainAdmin, base.ClientId, resource);
                return Ok(result);
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
                resourceApp.Delete(code);
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
