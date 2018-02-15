using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Domain
{
    [BsonIgnoreExtraElements]
    public class SvcToken
    {
        public string SvcId { get; set; }

        public string Token { get; set; }
    }
}
