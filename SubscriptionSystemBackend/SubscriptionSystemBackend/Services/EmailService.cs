using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailerSendConfig _config;
        private readonly ILogger<EmailService> _logger;
        private readonly HttpClient _httpClient;

        public EmailService(IOptions<MailerSendConfig> config, ILogger<EmailService> logger, HttpClient httpClient)
        {
            _config = config.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task SendInvitationEmailASync(string email, string token)
        {
            var inviteLink = $"http://localhost:3000/invite/confirm?token={token}";
            
            Console.WriteLine($"\n==================================================");
            Console.WriteLine($"INVITATION EMAIL SENT");
            Console.WriteLine($"==================================================");
            Console.WriteLine($"To: {email}");
            Console.WriteLine($"Subject: You're Invited to Join the Subscription System!");
            Console.WriteLine($"Invitation Link: {inviteLink}");
            Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Sent at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"==================================================\n");
            
            _logger.LogInformation("Invitation email sent to {Email} with token {Token}", email, token);
            
            if (!string.IsNullOrEmpty(_config?.ApiKey) && !string.IsNullOrEmpty(_config?.FromEmail))
            {
                try
                {
                    await SendViaMailerSend(email, inviteLink, token);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send email via MailerSend to {Email}, but invitation was logged", email);
                    Console.WriteLine($"  MailerSend failed: {ex.Message}");
                    Console.WriteLine($"   But invitation was logged above \n");
                }
            }
            else
            {
                _logger.LogInformation("MailerSend not configured, using console logging only for {Email}", email);
                Console.WriteLine($"  MailerSend not configured - using console logging only\n");
            }
        }

        private async Task SendViaMailerSend(string email, string inviteLink, string token)
        {
            var emailRequest = new
            {
                from = new
                {
                    email = _config.FromEmail,
                    name = _config.FromName
                },
                to = new[]
                {
                    new
                    {
                        email = email,
                        name = email.Split('@')[0]
                    }
                },
                subject = "You're Invited to Join the Subscription System!",
                text = $"You have been invited to join the Subscription System. Please click the following link to complete your registration: {inviteLink}",
                html = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #333;'>You're Invited to Join the Subscription System!</h2>
                        <p>Hello,</p>
                        <p>You have been invited to join the Subscription System. Please click the button below to complete your registration:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{inviteLink}' style='background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Complete Registration
                            </a>
                        </div>
                        <p>Or copy and paste this link into your browser:</p>
                        <p style='word-break: break-all; color: #666;'>{inviteLink}</p>
                        <p>If you did not expect this invitation, please ignore this email.</p>
                        <p>Best regards,<br>The Subscription System Team</p>
                    </div>"
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            var response = await _httpClient.PostAsJsonAsync("https://api.mailersend.com/v1/email", emailRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var messageId = response.Headers.GetValues("x-message-id").FirstOrDefault();
                _logger.LogInformation("Email sent successfully via MailerSend to {Email} with message ID: {MessageId}", email, messageId);
                Console.WriteLine($"Email sent successfully via MailerSend to {email}");
                if (!string.IsNullOrEmpty(messageId))
                {
                    Console.WriteLine($"   Message ID: {messageId}");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("MailerSend API error for {Email}. Status: {StatusCode}, Error: {Error}", 
                    email, response.StatusCode, errorContent);
                
                Console.WriteLine($" MailerSend API Error:");
                Console.WriteLine($"   Status: {response.StatusCode}");
                Console.WriteLine($"   Error: {errorContent}");
                throw new Exception($"MailerSend API error: {response.StatusCode} - {errorContent}");
            }
        }
    }
}
