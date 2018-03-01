using System;
using Xunit;

namespace Company.Tests
{
    using Core;
    using Data;
    using Domain;
    using MongoDB.Driver;

    public class CompanyTest
    {
        [Fact]
        public async void CreateCompany()
        {
            var CompanyService = new CompanyService(new MongoDataProvider("mongodb://localhost:27017"));
            var result = await CompanyService.CreateCompany(new CompanyEntity() { CompanyId = 5 });

            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
