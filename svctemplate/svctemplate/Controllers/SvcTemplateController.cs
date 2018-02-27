using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace svctemplate.Controllers
{
    using Core;
    using Domain;

    [Route("api/[controller]")]
    public class SvcTemplateController : Controller
    {
        private readonly SvcTemplateService svcTemplateService;

        public SvcTemplateController(SvcTemplateService svcTemplateService)
        {
            this.svcTemplateService = svcTemplateService;
        }

        // GET api/SvcTemplate/5
        [HttpGet("{id}")]
        public async Task<ExecutionResult<SvcTemplateEntity>> Get(long id)
        {
            return await this.svcTemplateService.GetSvcTemplate(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<ExecutionResult> Post(SvcTemplateEntity value)
        {
            return await this.svcTemplateService.CreateSvcTemplate(value);
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
