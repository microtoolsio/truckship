namespace Auth.Models
{
    public class LoginModel : SecuredModel
    {
        public string Login { get; set; }

        public string Hash { get; set; }
    }
}
