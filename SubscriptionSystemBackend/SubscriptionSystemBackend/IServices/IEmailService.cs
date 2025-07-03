namespace SubscriptionSystemBackend.Services
{
    public interface IEmailService
    {
        Task SendInvitationEmailASync(string email, string token);
    }
}
