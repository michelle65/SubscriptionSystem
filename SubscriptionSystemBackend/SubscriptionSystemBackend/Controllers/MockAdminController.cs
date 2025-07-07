using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Services;

namespace SubscriptionSystemBackend.Controllers
{
    [ApiController]
    [Route("api/mock-admin")]
    public class MockAdminController : Controller
    {
        private readonly IMockAdminService _mockAdminService;
        private readonly ILogger<MockAdminController> _logger;

        public MockAdminController(IMockAdminService mockAdminService, ILogger<MockAdminController> logger)
        {
            _mockAdminService = mockAdminService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMockAdmins()
        {
            try
            {
                var mockAdmins = await _mockAdminService.GetAllMockAdminsAsync();
                return Ok(mockAdmins);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mock administrators");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("tokens")]
        public async Task<IActionResult> GetAvailableTokens()
        {
            try
            {
                var tokens = await _mockAdminService.GetAvailableTokensAsync();
                return Ok(new { 
                    tokens = tokens.ToList(),
                    message = "Available mock admin tokens for testing",
                    usage = "Use any of these tokens in the 'token' field when registering an admin"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available tokens");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("validate/{token}")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            try
            {
                var mockAdmin = await _mockAdminService.ValidateTokenAsync(token);
                if (mockAdmin == null)
                {
                    var availableTokens = await _mockAdminService.GetAvailableTokensAsync();
                    return BadRequest(new { 
                        message = "Invalid token",
                        availableTokens = availableTokens.ToList()
                    });
                }

                return Ok(new { 
                    isValid = true,
                    admin = mockAdmin,
                    message = "Token is valid"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token: {Token}", token);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("permission/{token}/{permission}")]
        public async Task<IActionResult> CheckPermission(string token, string permission)
        {
            try
            {
                var hasPermission = await _mockAdminService.HasPermissionAsync(token, permission);
                var mockAdmin = await _mockAdminService.ValidateTokenAsync(token);

                return Ok(new { 
                    hasPermission,
                    permission,
                    adminName = mockAdmin?.Name ?? "Unknown",
                    message = hasPermission ? "Permission granted" : "Permission denied"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission for token: {Token}, permission: {Permission}", token, permission);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMockAdminById(int id)
        {
            try
            {
                var mockAdmin = await _mockAdminService.GetMockAdminByIdAsync(id);
                if (mockAdmin == null)
                    return NotFound(new { message = $"Mock admin with ID {id} not found" });

                return Ok(mockAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mock admin by ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetMockAdminByEmail(string email)
        {
            try
            {
                var mockAdmin = await _mockAdminService.GetMockAdminByEmailAsync(email);
                if (mockAdmin == null)
                    return NotFound(new { message = $"Mock admin with email {email} not found" });

                return Ok(mockAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mock admin by email: {Email}", email);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("token-status/{token}")]
        public async Task<IActionResult> CheckTokenStatus(string token)
        {
            try
            {
                var isUsed = await _mockAdminService.IsTokenUsedAsync(token);
                var isValid = await _mockAdminService.ValidateTokenAsync(token) != null;

                return Ok(new { 
                    token = token,
                    isValid = isValid,
                    isUsed = isUsed,
                    canRegister = isValid && !isUsed,
                    message = isUsed 
                        ? "Token has already been used for admin registration" 
                        : isValid 
                            ? "Token is valid and available for registration" 
                            : "Token is invalid"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking token status: {Token}", token);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 