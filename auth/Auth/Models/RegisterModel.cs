namespace Auth.Models
{
    public class RegisterModel : SecuredModel
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }
    }
}
