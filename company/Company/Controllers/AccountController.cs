using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Company.Core;
using Company.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly CompanyUserService companyUserService;
        private readonly SecurityService securityService;

        public AccountController(CompanyUserService companyUserService, SecurityService securityService)
        {
            this.companyUserService = companyUserService;
            this.securityService = securityService;
        }

        [HttpPost]
        [Route("signin")]
        [Authorize(Policy = "SvcUser")]
        public async Task<IActionResult> SignIn([FromBody]LoginModel login)
        {
            var principalInfo = User.GetInfo();
            var loginResult = await securityService.SignInUserToCompany(principalInfo.Login, login.CompanyIdentifier, login.Password);
            if (!loginResult.Success || loginResult.Value == null)
            {
                return new UnauthorizedResult();
            }

            return Ok(new ApiResponse<CompanyModel>() { Result = new CompanyModel() { CompanyIdentifier = loginResult.Value.CompanyIdentifier } });
        }
    }
}
