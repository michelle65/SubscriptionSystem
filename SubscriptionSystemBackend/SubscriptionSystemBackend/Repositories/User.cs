namespace SubscriptionSystemBackend.Repositories
{
    public class User
    {
        public int Id { get; internal set; }
        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string PasswordHash { get; internal set; }
        public string Role { get; internal set; }
        public bool IsConfirmed { get; internal set; }
    }
}