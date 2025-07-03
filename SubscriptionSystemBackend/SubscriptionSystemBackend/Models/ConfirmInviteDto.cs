namespace SubscriptionSystemBackend.Models
{
    public class ConfirmInviteDto
    {
        public string Token { get; set; }
        public object Password { get; internal set; }
        public object ConfirmPassword { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
    }
}
