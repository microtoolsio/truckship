using System;

namespace svctemplate.Core
{
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using MongoDB.Driver;

    public class SvcTemplateService
    {
        private const string SvcTemplateCollection = nameof(SvcTemplateEntity);

        private readonly MongoDataProvider mongoDataProvider;

        public SvcTemplateService(MongoDataProvider mongoDataProvider)
        {
            this.mongoDataProvider = mongoDataProvider;
            var options = new CreateIndexOptions() { Unique = true };
            var field = new StringFieldDefinition<SvcTemplateEntity>(nameof(SvcTemplateEntity.SvcTemplateId));
            var indexDefinition = new IndexKeysDefinitionBuilder<SvcTemplateEntity>().Ascending(field);
            this.mongoDataProvider.SvcTemplateDb.GetCollection<SvcTemplateEntity>(SvcTemplateCollection).Indexes
                .CreateOne(indexDefinition, options);
        }

        public async Task<ExecutionResult> CreateSvcTemplate(SvcTemplateEntity token)
        {
            var tokens = this.mongoDataProvider.SvcTemplateDb.GetCollection<SvcTemplateEntity>(SvcTemplateCollection);
            await tokens.InsertOneAsync(token);
            return new ExecutionResult();
        }

        public async Task<ExecutionResult<SvcTemplateEntity>> GetSvcTemplate(long id)
        {
            var collection = this.mongoDataProvider.SvcTemplateDb.GetCollection<SvcTemplateEntity>(SvcTemplateCollection);
            var cursor = await collection.FindAsync(x => x.SvcTemplateId == id);
            var entity = await cursor.FirstOrDefaultAsync();
            if (entity == null)
            {
                return new ExecutionResult<SvcTemplateEntity>(new ErrorInfo("404", "Entity not found"));
            }
            return new ExecutionResult<SvcTemplateEntity>(entity);
        }
    }
}
