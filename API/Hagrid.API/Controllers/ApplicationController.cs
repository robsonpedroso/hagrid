using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/application")]
    public class ApplicationController : BaseApiController
    {
        private readonly IApplicationApplication appAplication;

        public ApplicationController(IRequestInfoService info, IApplicationApplication appAplication)
            : base(info)
            => this.appAplication = appAplication;

        [HttpGet]
        [Route("{name}")]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                return Ok(appAplication.GetByName(name));
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
        [Route("")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult GetApplications()
        {
            try
            {
                return Ok(appAplication.GetApplications());
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
