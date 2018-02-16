using Xunit;
using MongoDB.Driver;
using Auth.Domain;

namespace Auth.Tests
{
    public class SupportTest
    {
        [Fact]
        public async void CreateSvcToken()
        {
            var db = (new MongoClient("mongodb://localhost:27017")).GetDatabase("auth");
            var tokens = db.GetCollection<SvcToken>("svctokens");
            await  tokens.InsertOneAsync(new SvcToken() { SvcId = "test", Token = "123" });
        }
    }
}
