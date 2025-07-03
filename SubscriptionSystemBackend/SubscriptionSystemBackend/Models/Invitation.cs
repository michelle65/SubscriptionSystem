namespace SubscriptionSystemBackend.Models
{
    public class Invitation
    {
        public string Email { get; internal set; }
        public string InvitationToken { get; internal set; }
        public bool IsUsed { get; internal set; }
    }
}