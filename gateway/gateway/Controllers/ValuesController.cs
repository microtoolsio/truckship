using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    using System;
    using System.Globalization;

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ClientHelper clientHelper;

        public ValuesController(ClientHelper clientHelper)
        {
            this.clientHelper = clientHelper;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            using (var client = clientHelper.GetServiceSecuredClient(User))
            {
                var authResp = await client.GetAsync("http://localhost:50765/api/account/test");
            }

            return Ok($"Gateway returns UTC Datetime {DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
