using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gateway.Configs;
using Gateway.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gateway
{
    public class SecretStorage
    {
        private readonly HttpClient client = new HttpClient();

        private readonly IOptionsMonitor<AppSettings> appSettings;

        private readonly SvcRouteTable routeTable;

        public SecretStorage(IOptionsMonitor<AppSettings> appSettings, SvcRouteTable routeTable)
        {
            this.appSettings = appSettings;
            this.routeTable = routeTable;
        }

        public async Task Init()
        {
            var resp = await client.PostAsync(this.routeTable.GetRoute(SvcRouteTable.GetSecret), new StringContent(JsonConvert.SerializeObject(new
            {
                SvcId = this.appSettings.CurrentValue.SvcId,
                SvcToken = this.appSettings.CurrentValue.Token
            }), Encoding.UTF8, "application/json"));

            var r = JsonConvert.DeserializeObject<ApiResponse<string>>(await resp.Content.ReadAsStringAsync());
            if (!r.Success || string.IsNullOrEmpty(r.Result))
            {
                throw new Exception("Can't start service because unable to retrieve secret information");
            }

            Secret = r.Result;
        }

        public string Secret { get; private set; }
    }
}
