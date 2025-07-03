using SubscriptionSystemBackend.IRepositories;
using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly List<Invitation> _invitations = new();

        public Task AddAsync(Invitation invitation)
        {
            _invitations.Add(invitation);
            return Task.CompletedTask;
        }

        public Task<Invitation?> GetByTokenAsync(string token)
        {
            return Task.FromResult(_invitations.FirstOrDefault(i => i.InvitationToken == token));
        }

        public Task<IEnumerable<Invitation>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Invitation>>(_invitations);
        }

        public Task SaveChangesAsync() => Task.CompletedTask;

        public Task AddAsync(IInvitationRepository invitation)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<IInvitationRepository>> IInvitationRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }

}
