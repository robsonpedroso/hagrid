using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/application-store")]
    public class ApplicationStoreController : BaseApiController
    {
        private IStoreApplication storeApp = null;
        private IApplicationStoreApplication appStore;

        public ApplicationStoreController(IRequestInfoService info, IStoreApplication storeApp, IApplicationStoreApplication appStore)
            : base(info)
        {
            this.storeApp = storeApp;
            this.appStore = appStore;
        }

        [HttpPost]
        [Route("")]
        [HashAuthorization]
        public IHttpActionResult SaveStore(DTO.Store store)
        {
            try
            {
                return Ok(storeApp.Save(store));
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
        [Route("connect-applications")]
        [HashAuthorization]
        public IHttpActionResult ConnectApplications(DTO.Store store)
        {
            try
            {
                return Ok(storeApp.ConnectApplications(store));
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
        [Route("store/{code}")]
        [JwtAuthorization(Roles = "Application, Member")]
        public IHttpActionResult GetByStoreCode(Guid code)
        {
            try
            {
                return Ok(appStore.GetByStore(code, base.ClientId));
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
