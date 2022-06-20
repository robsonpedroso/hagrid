using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Infrastructure.Services.Accounts
{
    public class AccountInfraService : IAccountInfraService
    {
        public DTO.AccountToken GetToken(string transfertoken, Guid clientId, string secret)
        {
            var client = new RestClient(Config.AccountApiBaseURL);
            var request = new RestRequest("/token", Method.Post);
            client.Authenticator = new HttpBasicAuthenticator(clientId.ToString(), secret);
            client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "transfer_token");
            request.AddParameter("transfer_token", transfertoken, ParameterType.GetOrPost);

            var response = client.Execute<DTO.AccountToken>(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException(response.Content);
                default:
                    throw new Exception(response.Content);
            }
        }

        public object GetChangePasswordToken(string token, Guid clientId, string secret)
        {
            var client = new RestClient(Config.AccountApiBaseURL);
            var request = new RestRequest("/token", Method.Post);
            client.Authenticator = new HttpBasicAuthenticator(clientId.ToString(), secret);
            client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "change_password");
            request.AddParameter("change_token", token, ParameterType.GetOrPost);

            var response = client.Execute<object>(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException(response.Content);
                default:
                    throw new Exception(response.Content);
            }
        }

        public object ChangePassword(string token, string password, string newPassword, Guid clientId, string secret)
        {
            var client = new RestClient(Config.AccountApiBaseURL);
            client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", token));
            client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");

            var request = new RestRequest("/member/password-change", Method.Post);
            request.AddParameter("password", password);
            request.AddParameter("password_new", newPassword);

            var response = client.Execute<object>(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException(response.Content);
                default:
                    throw new Exception(response.Content);
            }
        }
    }
}
