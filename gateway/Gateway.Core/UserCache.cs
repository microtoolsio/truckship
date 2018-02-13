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

        public async Task<byte[]> GetAsync(string session)
        {
            var res = await cache.GetAsync(session);
            return res;
        }

        public async Task SetAsync(string session, byte[] ticket, DistributedCacheEntryOptions opts = null)
        {
            await cache.SetAsync(session, ticket, opts);
        }

        public async Task RemoveAsync(string session)
        {
            await cache.RemoveAsync(session);
        }
    }
}
