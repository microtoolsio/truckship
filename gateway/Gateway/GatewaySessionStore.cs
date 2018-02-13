using System;
using System.Threading.Tasks;
using Gateway.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Gateway
{
    public class GatewaySessionStore : ITicketStore
    {
        private readonly UserCache userCache;
        // TODO: configurable.
        private const int ExpirationHours = 1;

        public GatewaySessionStore(UserCache userCache)
        {
            this.userCache = userCache;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = Guid.NewGuid().ToString();
            await RenewAsync(key, ticket);
            return key;
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new DistributedCacheEntryOptions();

            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }
            options.SetSlidingExpiration(TimeSpan.FromHours(ExpirationHours));
            await userCache.SetAsync(key, JsonConvert.SerializeObject(ticket), options);
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var user = await this.userCache.GetAsync(key);
            if (string.IsNullOrEmpty(user))
            {
                // TODO: NOTE: We can make a call to auth service here.
                return null;
            }

            return JsonConvert.DeserializeObject<AuthenticationTicket>(user);
        }

        public async Task RemoveAsync(string key)
        {
            await userCache.RemoveAsync(key);
        }
    }
}
