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

            var token = new JwtSecurityToken(
                issuer: $"{appSettings.CurrentValue.SvcId}:{appSettings.CurrentValue.Token}",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            /* var t = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
                 .WithSecret(this.secretStorage.Secret)
                 .AddHeader(HeaderName.KeyId, this.secretStorage.Secret)
                 .AddClaim("svcId", appSettings.CurrentValue.SvcId)
                 .AddClaim("svcToken", appSettings.CurrentValue.Token);

             var token = t.Build();*/

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secretStorage.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: $"{appSettings.CurrentValue.SvcId}:{appSettings.CurrentValue.Token}",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
          /*  var token = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(this.secretStorage.Secret)
                .AddClaim(ClaimName.Issuer, user.FindFirst(ClaimTypes.Name).Value)
                .AddHeader(HeaderName.KeyId, this.secretStorage.Secret)
                .AddClaim("svcId", appSettings.CurrentValue.SvcId)
                .AddClaim("svcToken", appSettings.CurrentValue.Token)
                .Build();*/

            var authValue = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
            };

            return client;
        }
    }
}
