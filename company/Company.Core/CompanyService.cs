using System;

namespace Company.Core
{
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using MongoDB.Driver;

    public class CompanyService
    {
        private const string CompanyCollection = nameof(CompanyEntity);

        private readonly MongoDataProvider mongoDataProvider;

        public CompanyService(MongoDataProvider mongoDataProvider)
        {
            this.mongoDataProvider = mongoDataProvider;
            var options = new CreateIndexOptions() { Unique = true };
            var field = new StringFieldDefinition<CompanyEntity>(nameof(CompanyEntity.CompanyId));
            var indexDefinition = new IndexKeysDefinitionBuilder<CompanyEntity>().Ascending(field);
            this.mongoDataProvider.CompanyDb.GetCollection<CompanyEntity>(CompanyCollection).Indexes
                .CreateOne(indexDefinition, options);
        }

        public async Task<ExecutionResult> CreateCompany(CompanyEntity token)
        {
            var tokens = this.mongoDataProvider.CompanyDb.GetCollection<CompanyEntity>(CompanyCollection);
            await tokens.InsertOneAsync(token);
            return new ExecutionResult();
        }

        public async Task<ExecutionResult<CompanyEntity>> GetCompany(long id)
        {
            var collection = this.mongoDataProvider.CompanyDb.GetCollection<CompanyEntity>(CompanyCollection);
            var cursor = await collection.FindAsync(x => x.CompanyId == id);
            var entity = await cursor.FirstOrDefaultAsync();
            if (entity == null)
            {
                return new ExecutionResult<CompanyEntity>(new ErrorInfo("404", "Entity not found"));
            }
            return new ExecutionResult<CompanyEntity>(entity);
        }
    }
}
