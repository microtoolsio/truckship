using System.Collections.Concurrent;
using Gateway.Configs;
using Microsoft.Extensions.Options;

namespace Gateway
{
    public class SvcRouteTable
    {
        #region [Route keys]

        public const string SignIn = "signin";

        public const string Register = "register";

        #endregion

        private readonly IOptionsMonitor<RouteConfig> routeConfig;

        public SvcRouteTable(IOptionsMonitor<RouteConfig> routeConfig)
        {
            this.routeConfig = routeConfig;
            BindRoutes();
        }

        private readonly ConcurrentDictionary<string, string> routes = new ConcurrentDictionary<string, string>();

        private void BindRoutes()
        {
            routes.TryAdd(SignIn, routeConfig.CurrentValue.SignIn);
            routes.TryAdd(Register, routeConfig.CurrentValue.Register);
        }

        public string GetRoute(string key)
        {
            string res = string.Empty;

            routes.TryGetValue(key, out res);
            if (string.IsNullOrEmpty(res))
            {
                throw new RouteNotFoundException(key);
            }

            return res;
        }
    }
}
