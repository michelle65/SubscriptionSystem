using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SubscriptionSystemBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "Y";
        private readonly string _fromEmail = "";
        private readonly string _fromName = "Admin";

        public EmailService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task SendInvitationEmailASync(string email, string token)
        {
            var inviteLink = $"https://your-frontend.com/invite/confirm?token={token}";
            var content = new
            {
                from = new { email = _fromEmail, name = _fromName },
                to = new[] { new { email } },
                subject = "You're Invited!",
                text = $"Click to join: {inviteLink}",
                html = $"<p>Hello,</p><p>You have been invited. Click <a href=\"{inviteLink}\">here</a> to register.</p>"
            };

            var json = JsonSerializer.Serialize(content);
            var response = await _httpClient.PostAsync("https://api.mailersend.com/v1/email",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Email failed: {error}");
            }
        }

       
    }
}
