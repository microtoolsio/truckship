using MongoDB.Driver;

namespace Auth.Core
{
    public class MongoDataProvider
    {
        public MongoDataProvider(string connectionString)
        {
            AuthDb = (new MongoClient(connectionString)).GetDatabase("auth");
        }

        public IMongoDatabase AuthDb { get; }
    }
}
