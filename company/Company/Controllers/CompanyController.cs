using System.Threading.Tasks;
using Company.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    using Core;
    using Domain;

    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly CompanyService CompanyService;

        public CompanyController(CompanyService CompanyService)
        {
            this.CompanyService = CompanyService;
        }

        // GET api/Company/5
        [HttpGet("{id}")]
        public async Task<ExecutionResult<CompanyEntity>> Get(string id)
        {
            return await this.CompanyService.GetCompany(id);
        }

        // POST api/values
        [HttpPost]
        [Authorize(Policy = "SvcUser")]
        public async Task<ExecutionResult> Post(CreateCompanyModel company)
        {
            var principalInfo = User.GetInfo();

            CompanyEntity entity = new CompanyEntity()
            {
                CompanyName = company.Name,
                OwnerIdentifier = principalInfo.Login
            };
            return await this.CompanyService.CreateCompany(entity);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
