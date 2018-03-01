namespace Company.Domain
{
    public class CompanyUserEntity
    {
        public string CompanyIdentifier { get; set; }

        public string UserIdentifier { get; set; }

        public string PasswordHash { get; set; }

        public bool IsDefault { get; set; }

        public string Salt { get; set; }
    }
}
