namespace Company.Domain
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class CompanyEntity
    {
        public string Identifier { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }
    }
}
