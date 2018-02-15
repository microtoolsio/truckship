using System.Threading.Tasks;
using Auth.Domain;
using MongoDB.Driver;

namespace Auth.Core
{
    public class UserStorage
    {
        private readonly IMongoDatabase userDb;

        public UserStorage(string connectionString)
        {
            userDb = (new MongoClient(connectionString)).GetDatabase("auth");
        }

        public async Task<ExecutionResult> CreateUser(User user)
        {
            var users = userDb.GetCollection<User>("users");
            if ((await users.CountAsync(x => x.Login == user.Login)) > 0)
            {
                return new ExecutionResult<User>() { Error = "The user already exist" };
            }
            await users.InsertOneAsync(user);
            return new ExecutionResult();
        }

        public async Task<ExecutionResult<User>> GetUser(string login, string pass)
        {
            var users = userDb.GetCollection<User>("users");
            var user = await users.FindAsync(x => x.Login == login && x.PasswordHash == pass);
            return new ExecutionResult<User>() { Result = user.First() };
        }
    }
}
