using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Company.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]LoginModel login)
        {
            /*  User
              var res = await this.userStorage.GetUser(login.Login);
              ApiResponse<UserModel> resp = new ApiResponse<UserModel>() { Error = res.Error };
              if (res.Result != null)
              {
                  var hash = GetHashString(login.Password, Convert.FromBase64String(res.Result.Salt));
                  if (hash != res.Result.PasswordHash)
                  {
                      return new UnauthorizedResult();
                  }
                  else
                  {
                      resp.Result = new UserModel() { Login = res.Result.Login };
                  }
              }

              return Ok(resp);*/

            return Ok();
        }

        [Route("test")]
        public async Task<IActionResult> Test()
        {
            var claims = new List<Claim>
            {
                new Claim("login", "test")
            };

            var userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal pr = new ClaimsPrincipal(userIdentity);
            var t = pr.GetInfo();
            return Ok();
        }
    }
}
