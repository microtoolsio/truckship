using System;

namespace Company.Domain
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class CompanyEntity
    {
        public long CompanyId { get; set; }
    }
}
