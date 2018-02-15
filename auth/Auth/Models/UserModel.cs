namespace Auth.Models
{
    public class UserModel : SecuredModel
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }
    }
}
