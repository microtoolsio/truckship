using System.Threading.Tasks;
using Auth.Core;
using Auth.Domain;
using Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserStorage userStorage;

        public AccountController(UserStorage userStorage)
        {
            this.userStorage = userStorage;
        }

        public async Task<IActionResult> Register(UserModel user)
        {
            await this.userStorage.CreateUser(new User() { Login = user.Login, PasswordHash = user.PasswordHash });
            return Ok();
        }
    }
}