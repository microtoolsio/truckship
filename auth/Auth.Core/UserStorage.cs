using System.Threading.Tasks;
using Auth.Domain;
using MongoDB.Driver;

namespace Auth.Core
{
    public class UserStorage
    {
        private readonly MongoDataProvider dataProvider;

        public UserStorage(MongoDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

            var options = new CreateIndexOptions() { Unique = true };
            var field = new StringFieldDefinition<User>("Login");
            var indexDefinition = new IndexKeysDefinitionBuilder<User>().Ascending(field);
            this.dataProvider.AuthDb.GetCollection<User>("users").Indexes.CreateOne(indexDefinition, options);
        }

        public async Task<ExecutionResult> CreateUser(User user)
        {
            var users = this.dataProvider.AuthDb.GetCollection<User>("users");
            if ((await users.CountAsync(x => x.Login == user.Login)) > 0)
            {
                return new ExecutionResult<User>() { Error = "The user already exist" };
            }
            await users.InsertOneAsync(user);
            return new ExecutionResult();
        }

        public async Task<ExecutionResult<User>> GetUser(string login)
        {
            var users = this.dataProvider.AuthDb.GetCollection<User>("users");
            var user = await users.FindAsync(x => x.Login == login);
            var result = await user.FirstOrDefaultAsync();
            var executionResult = new ExecutionResult<User>() { Result = result };
            if (result == null)
            {
                executionResult.Error = "Not Found";
            }
            return executionResult;
        }


        public async Task<ExecutionResult> UpdateUser(User user)
        {
            var users = this.dataProvider.AuthDb.GetCollection<User>("users");
            var updateResult = await users.UpdateOneAsync(x => x.Login == user.Login,
                 Builders<User>.Update
                     .Set(x => x.FirstName, user.FirstName)
                     .Set(x => x.LastName, user.LastName));

            if (updateResult.ModifiedCount == 0)
            {
                return new ExecutionResult() { Error = "Not Found" };
            }
            return new ExecutionResult();
        }
    }
}
