using System;
using Company.Domain.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
            var field = new StringFieldDefinition<CompanyEntity>(nameof(CompanyEntity.Identifier));
            var indexDefinition = new IndexKeysDefinitionBuilder<CompanyEntity>().Ascending(field);
            this.mongoDataProvider.CompanyDb.GetCollection<CompanyEntity>(CompanyCollection).Indexes
                .CreateOne(indexDefinition, options);
        }

        public async Task<ExecutionResult<string>> CreateCompany(CompanyEntity token)
        {
            string identitfier = Guid.NewGuid().ToString();
            token.Identifier = identitfier;
            var tokens = this.mongoDataProvider.CompanyDb.GetCollection<CompanyEntity>(CompanyCollection);
            await tokens.InsertOneAsync(token);
            return new ExecutionResult<string>(identitfier);
        }

        public async Task<ExecutionResult<CompanyEntity>> GetCompany(string identifier)
        {
            var collection = this.mongoDataProvider.CompanyDb.GetCollection<CompanyEntity>(CompanyCollection);
            var cursor = await collection.FindAsync(x => x.Identifier == identifier);
            var entity = await cursor.FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(CompanyEntity), identifier);
            }
            return new ExecutionResult<CompanyEntity>(entity);
        }


    }
}
