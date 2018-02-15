using System.Threading.Tasks;
using Auth.Domain;
using MongoDB.Driver;

namespace Auth.Core
{
    public class SvcTokenStorage
    {
        private readonly MongoDataProvider dataProvider;

        public SvcTokenStorage(MongoDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

            var options = new CreateIndexOptions() { Unique = true };
            var field = new StringFieldDefinition<SvcToken>("SvcId");
            var indexDefinition = new IndexKeysDefinitionBuilder<SvcToken>().Ascending(field);
            this.dataProvider.AuthDb.GetCollection<SvcToken>("svctokens").Indexes.CreateOne(indexDefinition, options);
        }

        public async Task<ExecutionResult> CreateSvcToken(SvcToken token)
        {
            var tokens = this.dataProvider.AuthDb.GetCollection<SvcToken>("svctokens");
            await tokens.InsertOneAsync(token);
            return new ExecutionResult();
        }

        public async Task<ExecutionResult<SvcToken>> GetSvcToken(string svcId)
        {
            var tokens = this.dataProvider.AuthDb.GetCollection<SvcToken>("svctokens");
            var token = await tokens.FindAsync(x => x.SvcId == svcId);
            return new ExecutionResult<SvcToken>() { Result = token.First() };
        }
    }
}
