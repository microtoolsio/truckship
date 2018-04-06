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
        public async Task<IActionResult> Register([FromBody]CreateCompanyModel company)
        {
            var principalInfo = User.GetInfo();

            byte[] salt = new byte[128];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            CompanyEntity entity = new CompanyEntity()
            {
                CompanyName = company.Name,
                OwnerIdentifier = principalInfo.Login,
                Email = company.Email,
                Description = company.Description
            };

            var res = await this.companyService.CreateCompany(entity);

            if (res.Success)
            {
                CompanyUserEntity companyUser = new CompanyUserEntity()
                {
                    CompanyIdentifier = res.Value,
                    IsDefault = true,
                    PasswordHash = GetHashString(company.Password, salt),
                    UserIdentifier = entity.OwnerIdentifier
                };

                await this.companyUserService.CreateCompanyUser(companyUser);
                return Ok(new ApiResponse<string>(res.Value));
            }

            return Ok(new ApiResponse<string>() { Error = res.Errors.Count > 0 ? string.Join(';', res.Errors) : "Error occured" });
        }

        private string GetHashString(string pass, byte[] salt)
        {
            var h = KeyDerivation.Pbkdf2(pass, salt, KeyDerivationPrf.HMACSHA1,
                10000, 128);

            return Convert.ToBase64String(h);
        }
    }
}
