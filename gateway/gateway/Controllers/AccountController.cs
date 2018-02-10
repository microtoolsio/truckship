using System.Threading.Tasks;
using Gateway.Core;
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

        public async Task<IActionResult> SignIn()
        {
           // var user = this.userCache.Get()
            return Ok();
        }

        public async Task<IActionResult> SignOut()
        {
            return Ok();
        }
    }
}