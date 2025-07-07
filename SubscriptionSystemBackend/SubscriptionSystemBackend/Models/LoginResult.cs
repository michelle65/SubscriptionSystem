namespace SubscriptionSystemBackend.Models
{
    public class LoginResult
    {
        public string Token { get; set; } = string.Empty;
        
        public UserInfo User { get; set; } = new();
    }

    public class UserInfo
    {
        public string Email { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        
        public string Role { get; set; } = string.Empty;
        
        public bool HasFiscalCode { get; set; }
    }
} 