using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        public async Task<IActionResult> SignIn()
        {
            return Ok();
        }

        public async Task<IActionResult> SignOut()
        {
            return Ok();
        }
    }
}