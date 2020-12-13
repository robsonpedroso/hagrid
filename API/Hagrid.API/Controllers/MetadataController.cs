using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils.WebApi;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Core.Domain.Contracts.Services;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/metadata")]
    public class MetadataController : BaseApiController
    {
        private readonly IMetadataApplication metadataApp;

        public MetadataController(IRequestInfoService info, IMetadataApplication metadataApp)
            : base(info)
            => this.metadataApp = metadataApp;

        [HttpPost]
        [Route("")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Save(DTO.MetadataField field)
        {
            try
            {
                field = metadataApp.Save(field);

                return Ok(field);
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
        [Route("{code}")]
        [JwtAuthorization(Roles = "Application, Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Get(Guid code)
        {
            try
            {
                return Ok(metadataApp.Get(code));
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
        [JwtAuthorization(Roles = "Application, Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Search(DTO.SearchFilterMetadataField searchFilter)
        {
            try
            {
                return Ok(metadataApp.Search(searchFilter));
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
        [Route("{type}/{code}")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Remove([FromUri]FieldType type, Guid code)
        {
            try
            {
                //need get from with instanceName, to get right repository Store or Account.
                var app = GetApplication<IMetadataApplication>(type.ToLower());

                app.Remove(code);

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

        [HttpPut]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        [Route("{type}/{referenceCode}/value")]
        public IHttpActionResult SaveValue([FromUri]FieldType type, [FromUri]Guid referenceCode, IEnumerable<DTO.MetadataField> metadatas)
        {
            try
            {
                //need get from with instanceName, to get right repository Store or Account.
                var app = GetApplication<IMetadataApplication>(type.ToLower());

                app.SaveValue(type, referenceCode, metadatas);

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

        [HttpPost]
        [Route("sync")]
        [HashAuthorization]
        public IHttpActionResult SyncSave(DTO.MetadataField field)
        {
            return this.Save(field);
        }
        
        [HttpPut]
        [Route("{type}/{referenceCode}/value/sync")]
        [HashAuthorization]
        public IHttpActionResult SyncSaveValue([FromUri]FieldType type, [FromUri]Guid referenceCode, IEnumerable<DTO.MetadataField> metadatas)
        {
            return this.SaveValue(type, referenceCode, metadatas);
        }
    }
}