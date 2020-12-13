using Hagrid.Core.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Utils.WebApi;
using DTO = Hagrid.Core.Domain.DTO;
using System.Threading.Tasks;
using Hagrid.Core.Domain.Contracts.Services;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/requisition")]
    public class RequisitionController : BaseApiController
    {
        private readonly IRequisitionApplication requisitionApp;

        public RequisitionController(IRequestInfoService info, IRequisitionApplication requisitionApp)
            : base(info)
            => this.requisitionApp = requisitionApp;

        [HttpPost]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("internal")]
        public IHttpActionResult SaveInternal(DTO.DBRequisition requisition)
        {
            try
            {
                return Ok(requisitionApp.SaveDB(requisition));
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
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("with-file")]
        public async Task<IHttpActionResult> SaveWithFile()
        {
            try
            {
                var files = GetFiles(await Request.Content.ReadAsMultipartAsync());
                requisitionApp.SaveFile(files);

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

        [HttpGet]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("{pStoreCode}")]
        public IHttpActionResult Get(Guid pStoreCode)
        {
            try
            {
                var result = requisitionApp.GetByStore(pStoreCode);

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
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("{code}")]
        public IHttpActionResult Delete(Guid code)
        {
            try
            {
                requisitionApp.Delete(code);

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

        private List<DTO.FileRequisition> GetFiles(MultipartMemoryStreamProvider data)
        {
            var storeCode = data.Contents.Where(act => act.Headers.ContentDisposition.Name.Replace("\"", "") == "store_code");
            var filesType = data.Contents.Where(act => act.Headers.ContentDisposition.Name.Replace("\"", "") == "type_requisition");

            if (storeCode.Count() == 0)
                throw new ArgumentException("Loja inválida");

            var fileImportStoreCode = storeCode.First().ReadAsStringAsync().Result.AsGuid();

            if (fileImportStoreCode.IsEmpty())
                throw new ArgumentException("Referência inválida");

            if (filesType.Count() == 0)
                throw new ArgumentException("Tipo de arquivo inválido");

            var fileType = filesType.First().ReadAsStringAsync().Result;

            var files = new List<DTO.FileRequisition>();

            data.Contents.Where(file => file.Headers.ContentDisposition.Name.Replace("\"", "") == "file").ForEach(file =>
                files.Add(
                    new DTO.FileRequisition(
                        file.Headers.ContentDisposition.FileName.Replace("\"", ""),
                        file.Headers.ContentLength.AsInt(),
                        file.ReadAsByteArrayAsync().Result,
                        fileImportStoreCode,
                        fileType
                        )));

            if (files.Count == 0)
                throw new ArgumentException("Arquivo inválido");

            return files;
        }

        [HttpGet]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("csv/{code}")]
        public IHttpActionResult CSV(Guid code)
        {
            try
            {
                var result = requisitionApp.DownloadCsv(code);

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
    }
}
