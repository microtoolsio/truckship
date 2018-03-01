using System.Threading.Tasks;
using Company.Data;
using Company.Domain;
using Company.Domain.Exceptions;
using MongoDB.Driver;

namespace Company.Core
{
    public class CompanyUserService
    {
        private const string CompanyUserCollection = nameof(CompanyUserEntity);

        private readonly MongoDataProvider mongoDataProvider;

        public CompanyUserService(MongoDataProvider mongoDataProvider)
        {
            this.mongoDataProvider = mongoDataProvider;
        }

        public async Task<ExecutionResult> CreateCompanyUser(CompanyUserEntity companyUser)
        {
            var collection = this.mongoDataProvider.CompanyDb.GetCollection<CompanyUserEntity>(CompanyUserCollection);
            var c = await collection.CountAsync(x => x.UserIdentifier == companyUser.UserIdentifier && x.CompanyIdentifier == companyUser.CompanyIdentifier);
            if (c > 0)
            {
                return new ExecutionResult<CompanyUserEntity>(ErrorInfoFacory.CreateOne("The user already exist"));
            }

            await collection.InsertOneAsync(companyUser);
            return new ExecutionResult();
        }

        public async Task<ExecutionResult<CompanyUserEntity>> GetCompanyUser(string userIdentifier, string companyIdentifier)
        {
            var collection = this.mongoDataProvider.CompanyDb.GetCollection<CompanyUserEntity>(CompanyUserCollection);
            var cursor = await collection.FindAsync(x => x.UserIdentifier == userIdentifier && x.CompanyIdentifier == companyIdentifier);
            var entity = await cursor.FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }
            return new ExecutionResult<CompanyUserEntity>(entity);
        }
    }
}
