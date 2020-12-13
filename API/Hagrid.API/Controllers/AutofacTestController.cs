using Hagrid.Core.Domain.Contracts.Services;
using System.Web.Http;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("autofac/test")]
    public class AutofacTestController : BaseApiController
    {
        public AutofacTestController(IRequestInfoService info) : base(info) { }

        [HttpGet, Route("")]
        public IHttpActionResult Get() => Ok();
    }
}