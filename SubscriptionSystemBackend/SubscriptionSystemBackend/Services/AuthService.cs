using SubscriptionSystemBackend.Repositories;

namespace SubscriptionSystemBackend.Services
{
    public class AuthService:IAuthService
    {
        public string GenerateJwtToken(User user)
        {
            return $"mockuit-{user.Email}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        public string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

    }
}
