using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/store")]
    public class StoreController : BaseApiController
    {
        private readonly IStoreApplication storeApp;

        public StoreController(IRequestInfoService info, IStoreApplication storeApp)
            : base(info)
            => this.storeApp = storeApp;

        [HttpGet]
        [Route("{code}/address")]
        public IHttpActionResult GetStoreAddresses(Guid code)
        {
            try
            {
                return Ok(storeApp.GetStoreAddresses(code));
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
        [JwtAuthorization(Roles = "Member,Application")]
        [Route("{code}")]
        public IHttpActionResult GetStore(Guid code)
        {
            try
            {
                return Ok(storeApp.GetStore(code, base.ClientId));
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
        [Route("{term}/by-term")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult GetStoreByTerm(string term)
        {
            try
            {
                return Ok(storeApp.GetStore(term));
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
        [Route("search")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult SearchStore(DTO.SearchFilter searchFilter)
        {
            try
            {
                return Ok(storeApp.Search(searchFilter));
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
        [Route("encripted/{code}/{phrase}")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult GetEncriptedStore(Guid code, string phrase)
        {
            try
            {
                return Ok(storeApp.GetEncriptedStore(code, phrase));
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

        [HttpPut]
        [Route("")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Update(DTO.Store store)
        {
            try
            {
                return Ok(storeApp.Update(store));
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
        [Route("upload/{code}")]
        public async Task<IHttpActionResult> UploadLogo(Guid code)
        {
            try
            {
                var byteImage = await Request.Content.ReadAsByteArrayAsync();

                if (byteImage.Length > 0)
                {
                    var storeInfo = storeApp.UploadLogo(byteImage, code);
                    return Ok(storeInfo);
                }
                else
                {
                    throw new ArgumentException("Selecione um arquivo válido!");
                }
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

        [HttpPut]
        [Route("sync")]
        [HashAuthorization]
        public IHttpActionResult SyncUpdate(DTO.Store store)
        {
            return this.Update(store);
        }

        [HttpGet]
        [Route("by-cnpj/{cnpj}")]
        [JwtAuthorization(Roles = "Application")]
        public IHttpActionResult GetStore(string cnpj)
        {
            try
            {
                return Ok(storeApp.GetByCnpj(cnpj));
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