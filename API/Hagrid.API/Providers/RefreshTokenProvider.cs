using Microsoft.Owin.Security.Infrastructure;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hagrid.API.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            try
            {
                if (context.Ticket.Properties.AllowRefresh.HasValue && context.Ticket.Properties.AllowRefresh.Value)
                {
                    var refreshTokenApp = context.OwinContext.Resolve<IRefreshTokenApplication>();
                    var appStore = context.OwinContext.Get<ApplicationStore>("ApplicationStore");
                    Guid ownerCode = appStore.Code;

                    if (context.Ticket.Identity.Claims.Any(c => c.Type.Equals(ClaimTypes.Role) && c.Value.Equals("Member")))
                        ownerCode = new Guid(context.Ticket.Identity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Sid)).Value);

                    var refreshToken = new RefreshToken(ownerCode, appStore);

                    context.Ticket.Properties.IssuedUtc = refreshToken.GeneratedUtc;
                    context.Ticket.Properties.ExpiresUtc = refreshToken.ExpiresUtc;

                    refreshToken.Ticket = context.SerializeTicket();


                    refreshTokenApp.Save(refreshToken);

                    context.SetToken(refreshToken.Code.EncodeURIComponent());
                }
            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            try
            {
                var refreshTokenApp = context.OwinContext.Resolve<IRefreshTokenApplication>();
                var appStore = context.OwinContext.Get<ApplicationStore>("ApplicationStore");

                if (appStore != null)
                {
                    if (appStore.ClientAuthType == ClientAuthType.JavaScript)
                    {
                        if (!context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new string[] { context.Request.Headers["Origin"] });
                    }
                }

                var token = refreshTokenApp.Get(context.Token.URLDecode());

                if (token != null && 
                    ((token.ApplicationStoreCode.HasValue && token.ApplicationStoreCode.Value == appStore.Code) || 
                    (!appStore.Store.IsNull() && appStore.Store.IsMain)))
                {
                    context.DeserializeTicket(token.Ticket);
                    refreshTokenApp.Delete(token);
                }
            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
            }
        }

        #region "  Async  "

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            return Task.Run(() => Create(context));
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            return Task.Run(() => Receive(context));
        }

        #endregion
    }
}