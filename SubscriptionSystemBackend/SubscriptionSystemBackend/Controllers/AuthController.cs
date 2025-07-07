using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Models;
using Microsoft.Extensions.Logging;

namespace SubscriptionSystemBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IUserService userService, ILogger<AuthController> logger) => (_userService, _logger) = (userService, logger);
        
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto dto)
        {
            try
        {
           await _userService.RegisterAdminAsync(dto);
                return Ok(new { message = "Administrator registered successfully. Please log in to continue." });
        }
            catch (UnauthorizedAccessException ex)
            {
                if (ex.Message.Contains("already been used for registration"))
                {
                    return BadRequest(new { 
                        message = "Admin token already used. Only one administrator can be registered with this token.",
                        error = "TOKEN_ALREADY_USED",
                        details = "This admin token has been used for a previous registration. Please contact system administrator for a new token."
                    });
                }
                
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering administrator");
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var loginResult = await _userService.LoginAsync(dto);
                return Ok(new { 
                    token = loginResult.Token, 
                    user = loginResult.User,
                    message = "Login successful" 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
