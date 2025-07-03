using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.Repositories;

namespace SubscriptionSystemBackend.IServices
{
    public interface IUserService
    {
        Task RegisterAdminAsync(RegisterAdminDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task InviteUsersAsync(string[] emails, string adminEmail);
        Task ConfirmInviteAsync(ConfirmInviteDto dto);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
 