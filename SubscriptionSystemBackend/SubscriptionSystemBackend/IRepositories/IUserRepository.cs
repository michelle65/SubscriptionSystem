namespace SubscriptionSystemBackend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task SaveChangesAsync();
        Task UpdateAsync(User user);
    }
}
