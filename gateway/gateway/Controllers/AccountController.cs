using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Gateway.Core;
using Gateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserCache userCache;
        private readonly HttpClient client = new HttpClient();

        public AccountController(UserCache userCache)
        {
            this.userCache = userCache;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]UserModel user)
        {
            if (LoginUser(user.Login, user.Password))
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login)
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