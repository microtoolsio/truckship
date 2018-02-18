using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Domain
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }
    }
}
