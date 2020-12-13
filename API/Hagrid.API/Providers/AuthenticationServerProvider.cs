using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Policies;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.API.Providers
{
    public class AuthenticationServerProvider : OAuthAuthorizationServerProvider
    {
        private void ValidateClientAuthenticationSync(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                var appStoApp = context.OwinContext.Resolve<IApplicationStoreApplication>();


                if (!context.TryGetBasicCredentials(out string clientId, out string clientSecret))
                    context.TryGetFormCredentials(out clientId, out clientSecret);

                if (appStoApp.Authenticate(context.ClientId, clientSecret, context.Request.Headers["Origin"], out string message, out ApplicationStore appStore))
                {
                    context.OwinContext.Set<ApplicationStore>("ApplicationStore", appStore);

                    if (!context.Request.Headers["Url-Back"].IsNullOrWhiteSpace())
                        context.OwinContext.Set<String>("UrlBack", context.Request.Headers["Url-Back"]);

                    context.Validated();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(message))
                        context.SetError("invalid_clientId", message);
                }
            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
                context.SetError(string.Format("Message: {0} | StackTrace: {1}", ex.GetBaseException().Message, ex.StackTrace));
            }
        }

        private void GrantResourceOwnerCredentialsSync(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ApplicationStore appStore = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(context.UserName))
                {
                    appStore = context.OwinContext.Get<ApplicationStore>("ApplicationStore");

                    var urlBack = context.OwinContext.Get<String>("UrlBack");
                    var accApp = context.OwinContext.Resolve<IAccountApplication>();
                    var account = accApp.Authenticate(context.UserName, context.Password, appStore, out bool requiresPasswordChange, urlBack);

                    GenerateMemberAccessToken(context, account, requiresPasswordChange);
                }
            }
            catch (LockedUpMemberException ex)
            {
                context.SetError("locked_member", ex.Message);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "create_password_is_needed")
                {
                    var storeCode = !appStore.IsNull() && !appStore.Store.IsNull() && !appStore.Store.Code.IsEmpty() ? appStore.Store.Code.ToString() : string.Empty;

                    context.SetError(ex.Message,
                        string.Format("member '{0}' needs to create password.", context.UserName),
                        string.Format("{0}/#/new-store/notification/{1}", Config.AccountsSiteURL, storeCode));
                }
                else
                {
                    context.SetError("invalid_grant");
                }
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
            }
        }

        private void GenerateMemberAccessToken(BaseValidatingTicketContext<OAuthAuthorizationServerOptions> context, Account account, bool requiresPasswordChange = false, string name = null, Guid? clientId = null)
        {
            try
            {
                if (!account.IsNull() && !account.Code.IsEmpty())
                {
                    ClaimsIdentity identity;
                    IEnumerable<string> scope;

                    if (context is OAuthGrantResourceOwnerCredentialsContext)
                    {
                        clientId = ((OAuthGrantResourceOwnerCredentialsContext)context).ClientId.AsGuid();
                        name = ((OAuthGrantResourceOwnerCredentialsContext)context).UserName;
                        scope = ((OAuthGrantResourceOwnerCredentialsContext)context).Scope;
                    }
                    else if (name.IsNullOrWhiteSpace())
                    {
                        throw new ArgumentException("name is required.");
                    }
                    else if (clientId.IsNull())
                    {
                        throw new ArgumentException("clientId is required.");
                    }
                    else if (!context.Ticket.IsNull())
                    {
                        scope = context.Ticket.Identity.Claims.Where(c => c.Type.Contains("urn:oauth:scope")).Select(x => x.Value).ToList<string>();
                    }
                    else
                    {
                        scope = new List<string>() { "" };
                    }

                    identity = new ClaimsIdentity(new GenericIdentity(name, OAuthDefaults.AuthenticationType));
                    identity.AddClaims(scope.Select(x => new Claim("urn:oauth:scope", x)));
                    identity.AddClaim(new Claim(ClaimTypes.System, clientId.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Member"));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, account.Code.ToString()));


                    context.OwinContext.Set<Guid>("MemberCode", account.Code);
                    context.OwinContext.Set<bool>("requires_password_change", requiresPasswordChange);
                    context.OwinContext.Set<DTO.AccountToken>("account", new DTO.AccountToken(account));

                    var currentAppStore = context.OwinContext.Get<ApplicationStore>("ApplicationStore");

                    if (!currentAppStore.IsNull())
                    {
                        identity.AddClaim(new Claim("app:store_code", currentAppStore.StoreCode.ToString()));
                    }

                    if (!account.AccountRoles.IsNull() && account.AccountRoles.Count > 0)
                    {
                        List<Guid> stores = new List<Guid>();
                        bool isAdminStores = false;

                        account.AccountRoles.ForEach(ar =>
                        {
                            if (ar.Role.Permissions.Any(p => p.Status &&
                                p.Resource.ApplicationCode == currentAppStore.ApplicationCode &&
                                p.Resource.Application.Status))
                            {
                                stores.Add(ar.Role.StoreCode);

                                if (ar.Role.Store.IsMain &&
                                        Config.AdminApplications.ToList().Contains(currentAppStore.Application.Name))
                                    isAdminStores = true;
                            }
                        });

                        if (!stores.IsNull() && stores.Count() > 0)
                            context.OwinContext.Set<IEnumerable<Guid>>("stores", stores.Distinct().ToList());


                        if (isAdminStores)
                            identity.AddClaim(new Claim("app:member_type", "MainAdmin"));
                        else if (!stores.IsNull() && stores.Count() > 0)
                            identity.AddClaim(new Claim("app:member_type", "Merchant"));
                    }
                    else
                    {
                        identity.AddClaim(new Claim("app:member_type", "Customer"));
                    }

                    var paramsDic = new Dictionary<string, string>();
                    var properties = new AuthenticationProperties(paramsDic) { AllowRefresh = true };

                    var ticket = new AuthenticationTicket(identity, properties);
                    context.Validated(ticket);
                }

            }
            catch (LockedUpMemberException ex)
            {
                context.SetError("locked_member", ex.Message);
            }
            catch (ArgumentException ex)
            {
                context.SetError("invalid_grant", ex.Message);
            }
        }

        private void GrantClientCredentialsSync(OAuthGrantClientCredentialsContext context)
        {
            try
            {
                var appStoApp = context.OwinContext.Resolve<IApplicationStoreApplication>();

                var appStore = context.OwinContext.Get<ApplicationStore>("ApplicationStore");

                GenerateApplicationAccessToken(context, appStore);
            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
                context.SetError(string.Format("Message: {0} | StackTrace: {1}", ex.GetBaseException().Message, ex.StackTrace));
            }
        }

        private void GenerateApplicationAccessToken(BaseValidatingTicketContext<OAuthAuthorizationServerOptions> context, ApplicationStore appStore, Guid? clientId = null)
        {
            try
            {
                ClaimsIdentity identity;
                IEnumerable<string> scope;

                if (context is OAuthGrantClientCredentialsContext)
                {
                    clientId = ((OAuthGrantClientCredentialsContext)context).ClientId.AsGuid();
                    scope = ((OAuthGrantClientCredentialsContext)context).Scope;
                }
                else if (clientId.IsNull())
                {
                    throw new ArgumentException("clientId is required.");
                }
                else
                {
                    scope = context.Ticket.Identity.Claims.Where(c => c.Type.Contains("urn:oauth:scope")).Select(x => x.Value).ToList<string>();
                }

                identity = new ClaimsIdentity(new GenericIdentity(clientId.ToString(), OAuthDefaults.AuthenticationType));
                identity.AddClaims(scope.Select(x => new Claim("urn:oauth:scope", x)));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Application"));
                identity.AddClaim(new Claim("app:name", appStore.Store.Name));
                identity.AddClaim(new Claim("app:auth_type", appStore.ClientAuthType.ToString()));
                identity.AddClaim(new Claim("app:store_code", appStore.Store.Code.ToString()));

                var ticket = new AuthenticationTicket(identity, new AuthenticationProperties() { AllowRefresh = true });
                context.Validated(ticket);
            }
            catch (ArgumentException ex)
            {
                context.SetError("invalid_grant", ex.Message);
            }
        }

        private void GrantRefreshTokenSync(OAuthGrantRefreshTokenContext context)
        {
            try
            {
                var accApp = context.OwinContext.Resolve<IAccountApplication>();
                var appStoApp = context.OwinContext.Resolve<IApplicationStoreApplication>();

                var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

                var role = newIdentity.Claims.FirstOrDefault(c => c.Type.Contains("role")).Value;

                switch (role)
                {
                    case "Member":
                        var account = accApp.GetAccount(newIdentity.Claims.FirstOrDefault(c => c.Type.Contains("sid")).Value.AsGuid(), context.ClientId.AsGuid(), includeRole: true);

                        if (!account.IsNull() && account.Status)
                            GenerateMemberAccessToken(context, account, false, newIdentity.Name, context.ClientId.AsGuid());
                        else
                            context.SetError("unauthorized", "refresh_token unauthorized for this application.");
                        break;
                    case "Application":

                        var clientId = newIdentity.Claims.FirstOrDefault(c => c.Type.Contains("name")).Value.AsGuid();

                        var appStore = appStoApp.GetByClientId(clientId);

                        if (!appStore.IsNull())
                            GenerateApplicationAccessToken(context, appStore, clientId);
                        else
                            context.SetError("unauthorized", "refresh_token unauthorized for this application.");
                        break;
                    default:
                        context.SetError("unauthorized", "refresh_token unauthorized for this application.");
                        break;
                }

            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
                context.SetError(string.Format("Message: {0} | StackTrace: {1}", ex.GetBaseException().Message, ex.StackTrace));
            }
        }

        private void TokenEndpointSync(OAuthTokenEndpointContext context)
        {
            var bannedParams = new string[] { ".refresh", ".issued", ".expires" };

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary.Where(p => !bannedParams.Contains(p.Key)))
            {
                context.AdditionalResponseParameters.Add(property.Key.CleanTypedKey(), property.ConvertFromTypedKey());
            }

            var requires_password_change = context.OwinContext.Get<bool>("requires_password_change");
            if (!requires_password_change.IsNull())
                context.AdditionalResponseParameters.Add("requires_password_change".CleanTypedKey(), requires_password_change);

            var account = context.OwinContext.Get<DTO.AccountToken>("account");
            if (!account.IsNull())
                context.AdditionalResponseParameters.Add("account".CleanTypedKey(), account.ToJsonString());

            var stores = context.OwinContext.Get<IEnumerable<Guid>>("stores");
            if (!stores.IsNull())
                context.AdditionalResponseParameters.Add("stores".CleanTypedKey(), stores.ToJsonString());
        }

        private void AuthorizeEndpointSync(OAuthAuthorizeEndpointContext context)
        {
            if (context.OwinContext.Authentication.AuthenticationResponseGrant != null)
                context.RequestCompleted();
        }

        private void ValidateClientRedirectUriSync(OAuthValidateClientRedirectUriContext context)
        {
            try
            {
                var scope = context.Request.Query["scope"];

                if (!string.IsNullOrWhiteSpace(scope))
                {
                    switch (scope)
                    {
                        case "client_credentials":
                            ValidateClientRedirectUriClientCredentials(context);
                            break;
                        default:
                            context.SetError("unsupported_scope", string.Format("scope '{0}' is not supported.", scope));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
                context.SetError(string.Format("Message: {0} | StackTrace: {1}", ex.GetBaseException().Message, ex.StackTrace));
            }
        }

        #region "  ValidateClientRedirectUriSync  "

        private void ValidateClientRedirectUriClientCredentials(OAuthValidateClientRedirectUriContext context)
        {
            var appStoApp = context.OwinContext.Resolve<IApplicationStoreApplication>();

            if (!string.IsNullOrWhiteSpace(context.ClientId))
            {
                if (appStoApp.Exists(context.ClientId))
                {
                    if (!string.IsNullOrWhiteSpace(context.RedirectUri))
                    {
                        if (Config.AllowedRedirectUris.Contains(new Uri(context.RedirectUri).GetLeftPart(UriPartial.Authority)))
                        {
                            var identity = new System.Security.Claims.ClaimsIdentity("Bearer");
                            identity.AddClaim(new Claim(ClaimTypes.Role, "Application"));
                            identity.AddClaim(new Claim(ClaimTypes.Name, context.ClientId));
                            identity.AddClaim(new Claim(ClaimTypes.Uri, context.RedirectUri));

                            context.OwinContext.Authentication.SignIn(new AuthenticationProperties() { AllowRefresh = true }, identity);
                            context.Validated(context.RedirectUri);
                        }
                        else
                        {
                            context.SetError("invalid_redirectUri", string.Format("the redirect_uri: {0} is not allowed.", context.RedirectUri));
                        }
                    }
                    else
                    {
                        context.SetError("invalid_redirectUri", "the redirect_uri must be specified.");
                    }
                }
                else
                {
                    context.SetError("invalid_clientId", string.Format("the client_id: {0} not exists.", context.ClientId));
                }
            }
            else
            {
                context.SetError("invalid_clientId", "client_id must be specified.");
            }
        }

        #endregion

        private void GrantCustomExtensionSync(OAuthGrantCustomExtensionContext context)
        {
            try
            {
                switch (context.GrantType)
                {
                    case "reset_password":
                        GrantResetPassword(context);
                        break;
                    case "change_password":
                        GrantChangePassword(context);
                        break;
                    case "transfer_token":
                        GrantTransferToken(context);
                        break;
                    case "reset_password_sms":
                        GrantResetAccountSMS(context);
                        break;
                    default:
                        context.SetError("unsupported_grant_type", string.Format("grant_type '{0}' is not supported.", context.GrantType));
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Set("Exception", ex);
                context.SetError(string.Format("Message: {0} | StackTrace: {1}", ex.GetBaseException().Message, ex.StackTrace));
            }
        }

        #region "  GrantCustomExtensionSync  "

        private void GrantResetPassword(OAuthGrantCustomExtensionContext context)
        {
            var appStoApp = context.OwinContext.Resolve<IApplicationStoreApplication>();
            var resetPasswordTokenApp = context.OwinContext.Resolve<IAccountResetPasswordTokenApplication>();

            var resetToken = context.Parameters["reset_token"];

            if (!string.IsNullOrWhiteSpace(resetToken))
            {
                var token = resetPasswordTokenApp.GetResetPasswordToken(resetToken);

                if (token != null)
                {
                    if (!token.IsExpired())
                    {
                        var identity = new ClaimsIdentity(new GenericIdentity(token.Code, OAuthDefaults.AuthenticationType));
                        identity.AddClaim(new Claim("urn:oauth:scope", string.Empty));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "ResetPassword"));
                        identity.AddClaim(new Claim(ClaimTypes.Sid, token.OwnerCode.Value.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.System, context.ClientId));

                        var app = appStoApp.Get(token.ApplicationStoreCode.Value);

                        var paramsDic = new Dictionary<string, string>();

                        if (!app.Store.IsNull())
                            paramsDic.Add("client_logo_url", app.Store.GetLogoURL());

                        var ticket = new AuthenticationTicket(identity, new AuthenticationProperties(paramsDic)
                        {
                            AllowRefresh = false,
                            IssuedUtc = token.GeneratedUtc,
                            ExpiresUtc = token.ExpiresUtc
                        });

                        context.Validated(ticket);
                    }
                    else
                    {
                        resetPasswordTokenApp.DeleteResetPasswordToken(token);
                        context.SetError("expired_reset_token", "reset_token already expired.");
                    }
                }
                else
                {
                    context.SetError("invalid_reset_token", "reset_token is invalid.");
                }
            }
            else
            {
                context.SetError("missing_parameter", "reset_token parameter is required for this grant_type.");
            }
        }

        private void GrantChangePassword(OAuthGrantCustomExtensionContext context)
        {
            var appStoApp = context.OwinContext.Resolve<IApplicationStoreApplication>();

            var changeToken = context.Parameters["change_token"];

            if (!string.IsNullOrWhiteSpace(changeToken))
            {
                ChangePasswordToken token = new ChangePasswordToken(changeToken);
                if (token.Validate())
                {
                    var identity = new ClaimsIdentity(new GenericIdentity(changeToken, OAuthDefaults.AuthenticationType));
                    identity.AddClaim(new Claim("urn:oauth:scope", string.Empty));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "ChangePassword"));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, token.OwnerCode.Value.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.System, token.ApplicationStoreCode.ToString()));

                    var applicationStore = appStoApp.Get(token.ApplicationStoreCode);

                    var paramsDic = new Dictionary<string, string>
                    {
                        { "client_logo_url", Store.GetLogoURL(applicationStore.Store.Code) },
                        { "change_password_message".MakeTypedKey(TypeCode.Boolean), token.ShowMessage.ToString() }
                    };

                    if (!string.IsNullOrWhiteSpace(token.UrlBack))
                        paramsDic.Add("url_back", token.UrlBack);

                    var ticket = new AuthenticationTicket(identity, new AuthenticationProperties(paramsDic)
                    {
                        AllowRefresh = false,
                        IssuedUtc = DateTime.UtcNow,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
                    });

                    context.Validated(ticket);
                }
                else
                {
                    context.SetError("expired_change_token", "change_token already expired.");
                }
            }
            else
            {
                context.SetError("missing_parameter", "change_token parameter is required for this grant_type.");
            }
        }

        private void GrantTransferToken(OAuthGrantCustomExtensionContext context)
        {
            var accApp = context.OwinContext.Resolve<IAccountApplication>();
            var transferTokenApp = context.OwinContext.Resolve<ITransferTokenApplication>();

            var transferTokenCode = context.Parameters["transfer_token"];

            if (!transferTokenCode.IsNullOrWhiteSpace())
            {

                var transferToken = transferTokenApp.Get(transferTokenCode);

                if (!transferToken.IsNull())
                {
                    var account = accApp.AuthenticateTransferToken(transferToken.OwnerCode.Value, context.ClientId.AsGuid());

                    if (!account.IsNull())
                    {
                        GenerateMemberAccessToken(context, account, false, transferToken.Name, context.ClientId.AsGuid());
                    }
                    else
                    {
                        context.SetError("unauthorized", "tranfer_token unauthorized for this application.");
                    }
                }
                else
                {
                    context.SetError("invalid_transfer_token", "transfer_token is invalid.");
                }
            }
            else
            {
                context.SetError("missing_parameter", "transfer_token parameter is required for this grant_type.");
            }
        }

        private void GrantResetAccountSMS(OAuthGrantCustomExtensionContext context)
        {
            var resetPasswordTokenApp = context.OwinContext.Resolve<IResetSMSTokenApplication>();

            var resetToken = context.Parameters["reset_token"];

            if (!string.IsNullOrWhiteSpace(resetToken))
            {
                var token = resetPasswordTokenApp.GetResetPasswordSMSToken(resetToken);

                if (token != null)
                {
                    var codeSMS = context.Parameters["sms_code"];

                    if (!token.IsExpired() && !token.IsUsed())
                    {
                        if (token.IsCodeValid(codeSMS))
                        {
                            var identity = new ClaimsIdentity(new GenericIdentity(token.Code, OAuthDefaults.AuthenticationType));
                            identity.AddClaim(new Claim("urn:oauth:scope", string.Empty));
                            identity.AddClaim(new Claim(ClaimTypes.Role, "ResetAccountSMS"));
                            identity.AddClaim(new Claim(ClaimTypes.Sid, token.OwnerCode.Value.ToString()));
                            identity.AddClaim(new Claim(ClaimTypes.System, context.ClientId));

                            var paramsDic = new Dictionary<string, string>();

                            if (!string.IsNullOrWhiteSpace(token.UrlBack))
                                paramsDic.Add("url_back", token.UrlBack);

                            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties(paramsDic)
                            {
                                AllowRefresh = false,
                                IssuedUtc = DateTime.UtcNow,
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
                            });

                            context.Validated(ticket);
                            token.SetUsed();
                            resetPasswordTokenApp.Update(token);
                        }
                        else
                        {
                            context.SetError("invalid_reset_code", "reset_token is invalid.");
                        }
                    }
                    else
                    {
                        context.SetError("expired_reset_token", "reset_token already expired.");
                    }
                }
                else
                {
                    context.SetError("invalid_reset_token", "reset_token is invalid.");
                }
            }
            else
            {
                context.SetError("missing_parameter", "reset_token parameter is required for this grant_type.");
            }
        }

        #endregion

        #region "  Async  "

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            return Task.Run(() => TokenEndpointSync(context));
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return Task.Run(() => ValidateClientAuthenticationSync(context));
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Run(() => GrantResourceOwnerCredentialsSync(context));
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            return Task.Run(() => GrantClientCredentialsSync(context));
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return Task.Run(() => GrantRefreshTokenSync(context));
        }

        public override Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            return Task.Run(() => GrantCustomExtensionSync(context));
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            return Task.Run(() => ValidateClientRedirectUriSync(context));
        }

        public override Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            return Task.Run(() => AuthorizeEndpointSync(context));
        }

        public override async Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            var url = context.Request.Uri.AbsolutePath;

            var paths = new string[] { "/v1/token", "/v2/token", "/v3/token", "/v4/token", "/v5/token",
                "/accounts/v1/token", "/accounts/v2/token", "/accounts/v3/token", "/accounts/v4/token", "/accounts/v5/token",
                "/accounts-manager/v1/token", "/accounts-manager/v2/token", "/accounts-manager/v3/token", "/accounts-manager/v4/token", "/accounts-manager/v5/token"
            };

            if (!string.IsNullOrEmpty(url) && paths.Contains(url))
            {
                context.MatchesTokenEndpoint();
                return;
            }
            await base.MatchEndpoint(context);
        }

        #endregion
    }
}