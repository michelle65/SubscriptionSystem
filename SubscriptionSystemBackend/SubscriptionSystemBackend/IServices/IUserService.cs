using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.Repositories;

namespace SubscriptionSystemBackend.IServices
{
    public interface IUserService
    {
        Task RegisterAdminAsync(RegisterAdminDto dto);
        Task<LoginResult> LoginAsync(LoginDto dto);
        Task InviteUsersAsync(string[] emails, string adminEmail);
        Task ConfirmInviteAsync(ConfirmInviteDto dto);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Invitation>> GetAllInvitationsAsync();
        Task UpdateFiscalCodeAsync(string email, string fiscalCode);
        Task<bool> CheckAdminFiscalCodeAsync(string email);
    }
}
 