using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Auth.Core;
using Auth.Domain;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    using System.Globalization;

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
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel register)
        {
            byte[] salt = new byte[128];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var res = await this.userStorage.CreateUser(new User()
            {
                FirstName = register.Name,
                Login = register.Login,
                PasswordHash = GetHashString(register.Password, salt),
                Salt = Convert.ToBase64String(salt)
            });
            return Ok(new ApiResponse() { Error = res.Error });
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]LoginModel login)
        {
            var res = await this.userStorage.GetUser(login.Login);
            ApiResponse<RegisterModel> resp = new ApiResponse<RegisterModel>() { Error = res.Error };
            if (res.Result != null)
            {
                var hash = GetHashString(login.Password, Convert.FromBase64String(res.Result.Salt));
                if (hash != res.Result.PasswordHash)
                {
                    return new UnauthorizedResult();
                }
                else
                {
                    resp.Result = new RegisterModel() { Login = res.Result.Login };
                }
            }

            return Ok(resp);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var login = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var res = await this.userStorage.GetUser(login);
            var resp = new ApiResponse<AccountModel>() { Error = res.Error };
            if (res.Result != null)
            {
                resp.Result = new AccountModel()
                {
                    Login = res.Result.Login,
                    FirstName = res.Result.FirstName,
                    LastName = res.Result.LastName,
                };
            }
            return Ok(resp);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateAccountModel model)
        {
            var login = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var res = await this.userStorage.UpdateUser(new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Login = login
            });
            return Ok(new ApiResponse() { Error = res.Error });
        }


        [HttpPost]
        [Route("getsecret")]
        public async Task<IActionResult> GetSecret([FromBody]SecuredModel m)
        {
            //TODO:NOTE: We can manage it later. We can store different secrets for deffirent app services or shared secret. We can update it by the schedule etc...
            var s = "B69B6D11-215C-467D-B51D-90CDFEA67336";
            return Ok(new ApiResponse<string> { Result = s });
        }

        [Route("test")]
        public async Task<IActionResult> Test()
        {
            return Ok($"UTC TIME {DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");

        }

        private string GetHashString(string pass, byte[] salt)
        {
            var h = KeyDerivation.Pbkdf2(password: pass, salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000, numBytesRequested: 128);

            return Convert.ToBase64String(h);
        }
    }
}