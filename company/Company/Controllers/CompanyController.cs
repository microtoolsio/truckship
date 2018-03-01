using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ExecutionResult> Post(CompanyEntity value)
        {
            return await this.CompanyService.CreateCompany(value);
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
