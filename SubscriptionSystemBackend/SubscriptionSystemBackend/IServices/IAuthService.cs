using SubscriptionSystemBackend.Repositories;

namespace SubscriptionSystemBackend.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        bool VerifyPassword(string password, string passwordHash);
        string HashPassword(string password);
    }
}
