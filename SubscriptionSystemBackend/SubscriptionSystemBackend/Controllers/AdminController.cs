using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;
        
        public AdminController(IUserService userService, ILogger<AdminController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("invitations")]
        public async Task<IActionResult> GetAllInvitations()
        {
            try
            {
                var invitations = await _userService.GetAllInvitationsAsync();
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invitations");
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var dashboardData = new
                {
                    totalUsers = users.Count(),
                    adminUsers = users.Count(u => u.Role == "Admin"),
                    regularUsers = users.Count(u => u.Role == "User"),
                    confirmedUsers = users.Count(u => u.IsConfirmed)
                };
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("invite-users")]
        public async Task<IActionResult> InviteUsers([FromBody] InviteDto dto)
        {
            try
            {
                var adminEmail = User.Identity!.Name!;
                
                await _userService.InviteUsersAsync(dto.Emails, adminEmail);
                return Ok(new { message = "Invitations sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending invitations");
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("fiscal-code")]
        public async Task<IActionResult> UpdateFiscalCode([FromBody] UpdateFiscalCodeDto dto)
        {
            try
            {
                var currentUserEmail = User.Identity?.Name;
                
                if (string.IsNullOrEmpty(currentUserEmail))
                {
                    _logger.LogError("User email not found in JWT token");
                    return BadRequest(new { message = "User email not found in token" });
                }
                
                _logger.LogInformation("Updating fiscal code for user: {Email}", currentUserEmail);
                
                await _userService.UpdateFiscalCodeAsync(currentUserEmail, dto.FiscalCode);
                return Ok(new { message = "Fiscal code updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating fiscal code");
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("fiscal-code-status")]
        public async Task<IActionResult> GetFiscalCodeStatus()
        {
            try
            {
                var currentUserEmail = User.Identity!.Name!;
                var hasFiscalCode = await _userService.CheckAdminFiscalCodeAsync(currentUserEmail);
                
                return Ok(new { 
                    hasFiscalCode,
                    message = hasFiscalCode ? "Fiscal code is set" : "Fiscal code is required"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking fiscal code status");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
