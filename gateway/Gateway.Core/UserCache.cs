using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Gateway.Core
{
    public class UserCache
    {
        private readonly IDistributedCache cache;

        public UserCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<string> GetAsync(string session)
        {
            var res = await cache.GetAsync(session);

            if (res != null)
            {
                return Encoding.UTF8.GetString(res);
            }

            return null;
        }

        public async Task SetAsync(string session, string ticket, DistributedCacheEntryOptions opts = null)
        {
            await cache.SetAsync(session, Encoding.UTF8.GetBytes(ticket), opts);
        }

        public async Task RemoveAsync(string session)
        {
            await cache.RemoveAsync(session);
        }
    }
}
