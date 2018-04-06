using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Company.Core;
using Company.Domain;
using Company.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly CompanyUserService companyUserService;
        private readonly SecurityService securityService;
        private readonly CompanyService companyService;

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

            return Ok(new ApiResponse<CompanySignInModel>() { Result = new CompanySignInModel() { CompanyIdentifier = loginResult.Value.CompanyIdentifier } });
        }

        [HttpPost]
        [Authorize(Policy = "SvcUser")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            var principalInfo = User.GetInfo();

            byte[] salt = new byte[128];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            CompanyUserEntity companyUser = new CompanyUserEntity()
            {
                CompanyIdentifier = model.CompanyIdentifier,
                IsDefault = true,
                PasswordHash = GetHashString(model.Password, salt),
                UserIdentifier = principalInfo.Login
            };

            await this.companyUserService.CreateCompanyUser(companyUser);
            return Ok(new ApiResponse());
        }


        private string GetHashString(string pass, byte[] salt)
        {
            var h = KeyDerivation.Pbkdf2(pass, salt, KeyDerivationPrf.HMACSHA1,
                10000, 128);

            return Convert.ToBase64String(h);
        }
    }
}
