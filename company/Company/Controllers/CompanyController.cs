using System.Threading.Tasks;
using Company.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    using Core;
    using Domain;

    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly CompanyService companyService;

        public CompanyController(CompanyService companyService)
        {
            this.companyService = companyService;
        }

        // GET api/Company/5
        [HttpGet("{id}")]
        public async Task<ExecutionResult<CompanyEntity>> Get(string id)
        {
            return await this.companyService.GetCompany(id);
        }

        public async Task<IActionResult> Register([FromBody] CreateCompanyModel company)
        {
            var principalInfo = User.GetInfo();

            CompanyEntity entity = new CompanyEntity()
            {
                CompanyName = company.Name,
                OwnerIdentifier = principalInfo.Login,
                Email = company.Email,
                Description = company.Description
            };

            var res = await this.companyService.CreateCompany(entity);
            return Ok(new ApiResponse<string>(res.Value) { Error = string.Join(';', res.Errors) });
        }
    }
}
