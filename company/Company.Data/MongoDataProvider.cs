using System;

namespace Company.Data
{
    using Domain;
    using MongoDB.Driver;

    public class MongoDataProvider
    {
        public MongoDataProvider(string connectionString)
        {
            CompanyDb = (new MongoClient(connectionString)).GetDatabase("Company");
        }

        public IMongoDatabase CompanyDb { get; }
    }
}
