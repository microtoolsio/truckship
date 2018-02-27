using System;
using Xunit;

namespace svctemplate.Tests
{
    using Core;
    using Data;
    using Domain;
    using MongoDB.Driver;

    public class SvcTemplateTest
    {
        [Fact]
        public async void CreateSvcTemplate()
        {
            var svcTemplateService = new SvcTemplateService(new MongoDataProvider("mongodb://localhost:27017"));
            var result = await svcTemplateService.CreateSvcTemplate(new SvcTemplateEntity() { SvcTemplateId = 5 });

            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
