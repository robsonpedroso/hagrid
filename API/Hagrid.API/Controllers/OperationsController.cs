using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;


namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/operations")]
    public class OperationsController : BaseApiController
    {
        private readonly IResourceApplication resourceApp;

        public OperationsController(IRequestInfoService info, IResourceApplication resourceApp)
            : base(info)
            => this.resourceApp = resourceApp;

        [HttpGet]
        [JwtAuthorization(Roles = "Member")]
        [Route("")]
        public IHttpActionResult GetOperations()
        {
            try
            {
                return Ok(resourceApp.GetOperations());
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
