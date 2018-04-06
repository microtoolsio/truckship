using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gateway.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/CompanyAccount")]
    public class CompanyController : Controller
    {
        private readonly ClientHelper clientHelper;


        private readonly SvcRouteTable routeTable;

        public CompanyController(SvcRouteTable routeTable, ClientHelper clientHelper)
        {
            this.routeTable = routeTable;
            this.clientHelper = clientHelper;
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CompanyCreateModel model)
        {
            using (var client = clientHelper.GetServiceSecuredClient(User))
            {
                var resp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.CompanyCreate),
                    new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                if (!resp.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)resp.StatusCode);
                }

                var content = await resp.Content.ReadAsStringAsync();
                return Content(content);
            }
        }
    }
}
