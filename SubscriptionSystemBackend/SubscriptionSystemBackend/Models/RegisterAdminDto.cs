namespace SubscriptionSystemBackend.Models
{
    public class RegisterAdminDto
    {
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; internal set; }
    }
}
