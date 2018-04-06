using System.Threading.Tasks;
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
    }
}
