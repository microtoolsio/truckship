using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Gateway.Configs;
using Gateway.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gateway

{
    public class ClientHelper
    {
        private readonly SecretStorage secretStorage;

        private readonly IOptionsMonitor<AppSettings> appSettings;

        public ClientHelper(SecretStorage secretStorage, IOptionsMonitor<AppSettings> appSettings)
        {
            this.secretStorage = secretStorage;
            this.appSettings = appSettings;
        }

        public HttpClient GetServiceSecuredClient()
        {
            //var token = encoder.Encode(headers, payload, this.secretStorage.Secret);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secretStorage.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim("svcId", appSettings.CurrentValue.SvcId),
                new Claim("svcToken", appSettings.CurrentValue.Token)
            };

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds, claims: claims);

            var authValue = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
            };

            return client;
        }

        public HttpClient GetSecuredClient(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                throw new NotAuthorizedException("can't get jwt because the user is not authenticated");
            }

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim("svcId", appSettings.CurrentValue.SvcId),
                new Claim("svcToken", appSettings.CurrentValue.Token),
                new Claim(ClaimTypes.Name, user.FindFirst(ClaimTypes.Name).Value)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secretStorage.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds, claims: claims);

            var authValue = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
            };

            return client;
        }
    }
}
