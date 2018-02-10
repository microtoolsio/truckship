using System.Text;
using System.Threading.Tasks;
using gateway.domain;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace gateway.core
{

    public class UserCache
    {
        private readonly IDistributedCache cache;

        public UserCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<User> Get(string session)
        {
            var res = await cache.GetAsync(session);

            if (res != null)
            {
                return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(res));
            }

            return null;
        }

        public async Task Set(string session, User user)
        {
            await cache.SetAsync(session, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user)));
        }
    }
}
