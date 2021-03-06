﻿using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gateway.Models;
using Gateway.Models.Company;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/CompanyAccount")]
    public class CompanyAccountController : Controller
    {
        private readonly ClientHelper clientHelper;

        private readonly SvcRouteTable routeTable;

        public CompanyAccountController(ClientHelper clientHelper, SvcRouteTable routeTable)
        {
            this.clientHelper = clientHelper;
            this.routeTable = routeTable;
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody]CompanyRegisterModel model)
        {
            using (var client = clientHelper.GetServiceSecuredClient(User))
            {
                var resp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.CompanyCreate),
                    new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                if (!resp.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)resp.StatusCode);
                }

                return Ok();
            }
        }

        [HttpPost]
        [Route("signin")]
        [Authorize]
        public async Task<IActionResult> SignIn([FromBody]CompanySignInModel company)
        {
            using (var client = clientHelper.GetServiceSecuredClient())
            {
                var authResp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.CompanySignIn), new StringContent(JsonConvert.SerializeObject(new
                {
                    CompanyIdentifier = company.CompanyIdentifier,
                    Password = company.Password,
                }), Encoding.UTF8, "application/json"));

                if (!authResp.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)authResp.StatusCode);
                }

                var u = JsonConvert.DeserializeObject<ApiResponse<CompanyResponseModel>>(await authResp.Content.ReadAsStringAsync());
                if (!u.Success || u.Result == null)
                {
                    return new UnauthorizedResult();
                }

                var claims = new List<Claim>
                {
                    new Claim("company", u.Result.CompanyIdentifier),
                };

                foreach (var userClaim in User.Claims)
                {
                    claims.Add(userClaim);
                }

                var userIdentity = new ClaimsIdentity(claims);

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }

            return Ok();
        }
    }
}
