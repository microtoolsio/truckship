using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Gateway.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserCache userCache;

        public AccountController(UserCache userCache)
        {
            this.userCache = userCache;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string login, string password)
        {
            if (LoginUser(login, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                //Just redirect to our index after logging in. 
                return Redirect("/");
            }

            return Ok();
        }

        public async Task<IActionResult> SignOut()
        {
            return Ok();
        }

        private bool LoginUser(string username, string password)
        {
            //As an example. This method would go to our data store and validate that the combination is correct. 
            //For now just return true. 
            return true;
        }
    }
}