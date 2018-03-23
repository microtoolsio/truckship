using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gateway.Configs;
using Gateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    using Microsoft.AspNetCore.Authentication.Cookies;

    [Produces(ApplicationJson)]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private const string ApplicationJson = "application/json";

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
                var authResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.SignIn), new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        Login = user.Login,
                        Password = user.Password,
                        Name = user.Name,
                    }), Encoding.UTF8, ApplicationJson));

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
                    new Claim(ClaimTypes.Name, u.Result.Login)
                };

                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
                }), Encoding.UTF8, ApplicationJson));

                return regResp.IsSuccessStatusCode ? (IActionResult)Ok() : (IActionResult)new UnauthorizedResult();
            }
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            using (var client = clientHelper.GetServiceSecuredClient(User))
            {
                var getResponse = await client.GetAsync(this.routeTable.GetRoute(SvcRouteTable.GetAccount));
                if (!getResponse.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)getResponse.StatusCode);
                }
                var content = await getResponse.Content.ReadAsStringAsync();
                return Content(content, ApplicationJson);
            }
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateAccountModel model)
        {
            using (var client = clientHelper.GetServiceSecuredClient(User))
            {
                var getResponse = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.UpdateAccount),
                    new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                        }), Encoding.UTF8, ApplicationJson));
                if (!getResponse.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)getResponse.StatusCode);
                }
                var content = await getResponse.Content.ReadAsStringAsync();
                return Content(content, ApplicationJson);
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