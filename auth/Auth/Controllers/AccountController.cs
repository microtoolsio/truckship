using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Auth.Core;
using Auth.Domain;
using Auth.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
            byte[] salt = new byte[128];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var res = await this.userStorage.CreateUser(new User()
            {
                Login = user.Login,
                PasswordHash = GetHashString(user.Password, salt),
                Salt = Convert.ToBase64String(salt)
            });
            return Ok(new ApiResponse() { Error = res.Error });
        }

        [HttpPost]
        [TypeFilter(typeof(SvcAuthFilter))]
        [Route("getuser")]
        public async Task<IActionResult> GetUser([FromBody]LoginModel login)
        {
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

            return Ok(resp);
        }

        [HttpPost]
        [TypeFilter(typeof(SvcAuthFilter))]
        [Route("getsecret")]
        public async Task<IActionResult> GetSecret([FromBody]SecuredModel m)
        {
            //TODO:NOTE: We can manage it later. We can store different secrets for deffirent app services or shared secret. We can update it by the schedule etc...
            var s = "B69B6D11-215C-467D-B51D-90CDFEA67336";
            return Ok(new ApiResponse<string> { Result = s });
        }

        private string GetHashString(string pass, byte[] salt)
        {
            var h = KeyDerivation.Pbkdf2(password: pass, salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000, numBytesRequested: 128);

            return Convert.ToBase64String(h);
        }
    }
}