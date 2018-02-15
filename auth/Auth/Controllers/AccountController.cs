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

        [HttpPost]
        [TypeFilter(typeof(SvcAuthFilter))]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserModel user)
        {
            var res = await this.userStorage.CreateUser(new User() { Login = user.Login, PasswordHash = user.PasswordHash });
            return Ok(new ApiResponse() { Error = res.Error });
        }

        [HttpPost]
        [TypeFilter(typeof(SvcAuthFilter))]
        [Route("getuser")]
        public async Task<IActionResult> GetUser([FromBody]LoginModel login)
        {
            var res = await this.userStorage.GetUser(login.Login, login.Hash);
            ApiResponse<UserModel> resp = new ApiResponse<UserModel>() { Error = res.Error };
            if (res.Result != null)
            {
                resp.Result = new UserModel() { Login = res.Result.Login, PasswordHash = res.Result.PasswordHash };
            }

            return Ok(resp);
        }
    }
}