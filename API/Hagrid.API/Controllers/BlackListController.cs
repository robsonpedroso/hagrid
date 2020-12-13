using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils.WebApi;
using System;
using System.Web.Http;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Controllers
{
    [RoutePrefix("v2/blacklist")]
    public class BlackListController : BaseApiController
    {
        private readonly IBlacklistApplication blacklistApp;

        public BlackListController(IRequestInfoService info, IBlacklistApplication blacklistApp)
            : base(info)
            => this.blacklistApp = blacklistApp;

        [HttpPost]
        [Route("")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Block(DTO.Blacklist blacklist)
        {
            try
            {
                blacklist.BlockedBy = base.AccountCode;
                return Ok(blacklistApp.Block(blacklist));
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
        [Route("unlock")]
        [JwtAuthorization(Roles = "Member", AppProperties = "member_type=MainAdmin")]
        public IHttpActionResult Unlock(DTO.Blacklist blacklist)
        {
            try
            {
                blacklist.BlockedBy = base.AccountCode;
                return Ok(blacklistApp.Unlock(blacklist));
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
