using System.Threading.Tasks;
using Auth.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Core;

namespace Auth.Core
{
    public class UserStorage
    {
        private readonly IMongoDatabase userDb;

        public UserStorage(string connectionString)
        {
            userDb = (new MongoClient(connectionString)).GetDatabase("auth");
        }

        public async Task CreateUser(User user)
        {
            var users = userDb.GetCollection<User>("users");
            await users.InsertOneAsync(user);
        }
    }
}
