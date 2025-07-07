using SubscriptionSystemBackend.Services;

namespace SubscriptionSystemBackend.IServices
{
    public interface IMockAdminService
    {
        Task<MockAdmin?> ValidateTokenAsync(string token);

        Task MarkTokenAsUsedAsync(string token);

        Task<bool> IsTokenUsedAsync(string token);

        Task<IEnumerable<MockAdmin>> GetAllMockAdminsAsync();

        Task<MockAdmin?> GetMockAdminByIdAsync(int id);

        Task<MockAdmin?> GetMockAdminByEmailAsync(string email);

        Task<bool> HasPermissionAsync(string token, string permission);

        Task<IEnumerable<string>> GetAvailableTokensAsync();

        Task LogAdminActivityAsync(string token, string activity);
    }
} 