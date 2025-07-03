using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.IRepositories
{
    public interface IInvitationRepository
    {
        Task<Invitation?> GetByTokenAsync(string token);
        Task<IEnumerable<IInvitationRepository>> GetAllAsync();
        Task SaveChangesAsync();
        Task AddAsync(Invitation invitation);
    }
}
