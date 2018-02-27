using System;

namespace svctemplate.Data
{
    using Domain;
    using MongoDB.Driver;

    public class MongoDataProvider
    {
        public MongoDataProvider(string connectionString)
        {
            SvcTemplateDb = (new MongoClient(connectionString)).GetDatabase("svcTemplate");
        }

        public IMongoDatabase SvcTemplateDb { get; }
    }
}
