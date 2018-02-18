using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Gateway.Core;
using Gateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        #region [ Fields ]

        private readonly UserCache userCache;

        private readonly SvcRouteTable routeTable;

        private readonly HttpClient client = new HttpClient();
        
        #endregion

        public AccountController(UserCache userCache, SvcRouteTable routeTable)
        {
            this.userCache = userCache;
            this.routeTable = routeTable;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]UserModel user)
        {
            var authResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.SignIn), new StringContent(JsonConvert.SerializeObject(new
            {
                Login = user.Login,
                Password = user.Password
            })));

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

            return Ok();
        }


        public async Task<IActionResult> SignOut()
        {
            return Ok();
        }

    }
}