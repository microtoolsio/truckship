using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gateway.Configs;
using Gateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        #region [ Fields ]

        private readonly SvcRouteTable routeTable;

        private readonly ClientHelper clientHelper;

        #endregion

        public AccountController(SvcRouteTable routeTable, ClientHelper clientHelper)
        {
            this.routeTable = routeTable;
            this.clientHelper = clientHelper;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]UserModel user)
        {
            using (var client = clientHelper.GetServiceSecuredClient())
            {
                var authResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.SignIn), new StringContent(JsonConvert.SerializeObject(new
                {
                    Login = user.Login,
                    Password = user.Password,
                }), Encoding.UTF8, "application/json"));

                if (!authResp.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)authResp.StatusCode);
                }

                var u = JsonConvert.DeserializeObject<ApiResponse<UserModel>>(await authResp.Content.ReadAsStringAsync());
                if (!u.Success || u.Result == null)
                {
                    return new UnauthorizedResult();
                }

                var claims = new List<Claim>
                {
                    new Claim("login", u.Result.Login)
                };

                var userIdentity = new ClaimsIdentity(claims);

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            using (var client = clientHelper.GetServiceSecuredClient())
            {
                var regResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.Register), new StringContent(JsonConvert.SerializeObject(new
                {
                    Login = user.Login,
                    Password = user.Password,
                }), Encoding.UTF8, "application/json"));

                return regResp.IsSuccessStatusCode ? (IActionResult)Ok() : (IActionResult)new UnauthorizedResult();
            }
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