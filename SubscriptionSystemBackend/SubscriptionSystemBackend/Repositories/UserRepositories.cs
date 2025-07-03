namespace SubscriptionSystemBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public Task<User?> GetByIdAsync(int id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult<IEnumerable<User>>(_users);
        }

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

}
