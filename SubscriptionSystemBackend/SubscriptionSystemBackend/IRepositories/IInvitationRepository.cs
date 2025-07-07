using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.IRepositories
{
    public interface IInvitationRepository
    {
        Task<Invitation?> GetByTokenAsync(string token);
        Task<IEnumerable<Invitation>> GetAllAsync();
        Task<IEnumerable<Invitation>> GetByEmailAsync(string email);
        Task SaveChangesAsync();
        Task AddAsync(Invitation invitation);
    }
}
