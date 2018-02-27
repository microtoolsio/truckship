using System;

namespace svctemplate.Domain
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class SvcTemplateEntity
    {
        public long SvcTemplateId { get; set; }
    }
}
