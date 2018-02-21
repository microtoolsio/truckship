using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gateway.Configs;
using Gateway.Models;
using JWT.Algorithms;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JWT;
using JWT.Builder;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        #region [ Fields ]

        private readonly SvcRouteTable routeTable;

        private readonly SecretStorage secretStorage;

        private readonly JwtStorage jwtStorage;

        private readonly HttpClient client = new HttpClient();


        private readonly IOptionsMonitor<AppSettings> appSettings;

        #endregion

        public AccountController(SvcRouteTable routeTable, IOptionsMonitor<AppSettings> appSettings, SecretStorage secretStorage, JwtStorage jwtStorage)
        {
            this.routeTable = routeTable;
            this.appSettings = appSettings;
            this.secretStorage = secretStorage;
            this.jwtStorage = jwtStorage;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]UserModel user)
        {
            var authResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.SignIn), new StringContent(JsonConvert.SerializeObject(new
            {
                Login = user.Login,
                Password = user.Password,
                SvcId = this.appSettings.CurrentValue.SvcId,
                SvcToken = this.appSettings.CurrentValue.Token
            }), Encoding.UTF8, "application/json"));

            var u = JsonConvert.DeserializeObject<ApiResponse<UserModel>>(await authResp.Content.ReadAsStringAsync());
            if (!u.Success || u.Result == null)
            {
                return new UnauthorizedResult();
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, u.Result.Login)
                };

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            var token = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(this.secretStorage.Secret)
                .AddClaim(ClaimName.Issuer, u.Result.Login)
                .Build();

            await jwtStorage.StoreJwt(principal.Identity.Name, token);
            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            var regResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.Register), new StringContent(JsonConvert.SerializeObject(new
            {
                Login = user.Login,
                Password = user.Password,
                SvcId = this.appSettings.CurrentValue.SvcId,
                SvcToken = this.appSettings.CurrentValue.Token
            }), Encoding.UTF8, "application/json"));

            return regResp.IsSuccessStatusCode ? (IActionResult)Ok() : (IActionResult)new UnauthorizedResult();
        }

        [HttpGet]
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

    }
}