using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.IServices;
using Microsoft.Extensions.Options;

namespace SubscriptionSystemBackend.Services
{
    public class MockAdminService : IMockAdminService
    {
        private readonly List<MockAdmin> _mockAdmins;
        private readonly ILogger<MockAdminService> _logger;
        private readonly MockAdminConfig _config;
        private bool _adminTokenUsed = false;

        public MockAdminService(ILogger<MockAdminService> logger, IOptions<MockAdminConfig> config)
        {
            _logger = logger;
            _config = config.Value;
            _mockAdmins = InitializeMockAdmins();
        }

        private List<MockAdmin> InitializeMockAdmins()
        {
            return new List<MockAdmin>
            {
                new MockAdmin
                {
                    Id = 1,
                    Token = _config.Token,
                    Name = _config.Name,
                    Email = _config.Email,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-30),
                    Permissions = new List<string> { "UserManagement", "InvitationManagement", "SystemAccess" }
                }
            };
        }

        public async Task<MockAdmin?> ValidateTokenAsync(string token)
        {
            _logger.LogInformation("Validating mock admin token: {Token}", token);
            
            if (_adminTokenUsed)
            {
                _logger.LogWarning("Admin token has already been used for registration: {Token}", token);
                return null;
            }
            
            var mockAdmin = _mockAdmins.FirstOrDefault(a => a.Token == token && a.IsActive);
            
            if (mockAdmin != null)
            {
                _logger.LogInformation("Mock admin token validated successfully for: {Name}", mockAdmin.Name);
                return await Task.FromResult(mockAdmin);
            }
            
            _logger.LogWarning("Invalid mock admin token: {Token}", token);
            return null;
        }

        public async Task MarkTokenAsUsedAsync(string token)
        {
            if (_mockAdmins.Any(a => a.Token == token))
            {
                _adminTokenUsed = true;
                _logger.LogInformation("Admin token marked as used: {Token}", token);
            }
            await Task.CompletedTask;
        }

        public async Task<bool> IsTokenUsedAsync(string token)
        {
            return await Task.FromResult(_adminTokenUsed);
        }

        public async Task<IEnumerable<MockAdmin>> GetAllMockAdminsAsync()
        {
            _logger.LogInformation("Retrieving all mock administrators");
            return await Task.FromResult(_mockAdmins.Where(a => a.IsActive));
        }

        public async Task<MockAdmin?> GetMockAdminByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving mock admin by ID: {Id}", id);
            var mockAdmin = _mockAdmins.FirstOrDefault(a => a.Id == id && a.IsActive);
            return await Task.FromResult(mockAdmin);
        }

        public async Task<MockAdmin?> GetMockAdminByEmailAsync(string email)
        {
            _logger.LogInformation("Retrieving mock admin by email: {Email}", email);
            var mockAdmin = _mockAdmins.FirstOrDefault(a => a.Email == email && a.IsActive);
            return await Task.FromResult(mockAdmin);
        }

        public async Task<bool> HasPermissionAsync(string token, string permission)
        {
            var mockAdmin = await ValidateTokenAsync(token);
            if (mockAdmin == null) return false;
            
            return mockAdmin.Permissions.Contains(permission);
        }

        public async Task<IEnumerable<string>> GetAvailableTokensAsync()
        {
            _logger.LogInformation("Retrieving available mock admin tokens");
            var tokens = _mockAdmins.Where(a => a.IsActive).Select(a => a.Token);
            return await Task.FromResult(tokens);
        }

        public async Task LogAdminActivityAsync(string token, string activity)
        {
            var mockAdmin = await ValidateTokenAsync(token);
            if (mockAdmin != null)
            {
                _logger.LogInformation("Mock Admin Activity - {AdminName}: {Activity}", mockAdmin.Name, activity);
            }
        }
    }

    public class MockAdmin
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
} 