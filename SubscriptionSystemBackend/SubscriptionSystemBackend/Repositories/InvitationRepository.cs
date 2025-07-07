using Microsoft.EntityFrameworkCore;
using SubscriptionSystemBackend.IRepositories;
using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.Data;

namespace SubscriptionSystemBackend.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly ApplicationDbContext _context;

        public InvitationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
        }

        public async Task<Invitation?> GetByTokenAsync(string token)
        {
            return await _context.Invitations.FirstOrDefaultAsync(i => i.InvitationToken == token);
        }

        public async Task<IEnumerable<Invitation>> GetAllAsync()
        {
            return await _context.Invitations.ToListAsync();
        }

        public async Task<IEnumerable<Invitation>> GetByEmailAsync(string email)
        {
            return await _context.Invitations
                .Where(i => i.Email == email)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Invitation invitation)
        {
            invitation.UsedAt = DateTime.UtcNow;
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync();
        }
    }
}
