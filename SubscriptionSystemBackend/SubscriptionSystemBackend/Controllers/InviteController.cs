using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.Controllers
{
    [ApiController]
    [Route("api/invite")]
    public class InviteController: Controller
    {
        private readonly IUserService _userService;
        
        public InviteController(IUserService userService) => _userService = userService;
        
        [Authorize(Roles = "Admin")]
        [HttpPost("send")]
        public async Task<IActionResult> SendInvites(InviteDto dto)
        {
            try
            {
                var adminEmail = User.Identity!.Name!;
                await _userService.InviteUsersAsync(dto.Emails, adminEmail);
                return Ok(new { message = "Invitations sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmInvite(ConfirmInviteDto dto)
        {
            try
            {
                Console.WriteLine($"=== INVITATION CONFIRMATION REQUEST ===");
                Console.WriteLine($"Token: {dto.Token}");
                Console.WriteLine($"FirstName: {dto.FirstName}");
                Console.WriteLine($"LastName: {dto.LastName}");
                Console.WriteLine($"Password: {dto.Password}");
                Console.WriteLine($"ConfirmPassword: {dto.ConfirmPassword}");
                Console.WriteLine($"=====================================");
                
                await _userService.ConfirmInviteAsync(dto);
                return Ok(new { message = "Invitation confirmed successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== INVITATION CONFIRMATION ERROR ===");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"===================================");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
