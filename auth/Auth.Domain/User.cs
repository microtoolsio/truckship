using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Domain
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }
        
        public string PasswordHash { get; set; }

        public string Salt { get; set; }
    }
}
