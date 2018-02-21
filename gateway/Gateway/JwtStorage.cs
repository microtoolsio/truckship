using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Gateway
{
    public class JwtStorage
    {
        private readonly IDistributedCache cache;

        public JwtStorage(IDistributedCache cache)
        {
            this.cache = cache;
        }

        private const string jwtTokenTemplate = "jwt_{0}";

        public async Task StoreJwt(string key, string jwt)
        {
            var k = string.Format(jwtTokenTemplate, key);

            var options = new DistributedCacheEntryOptions();
            //TODO: NOTE: Configuration
            options.SetSlidingExpiration(TimeSpan.FromHours(1));
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(jwt), options);
        }
    }
}
