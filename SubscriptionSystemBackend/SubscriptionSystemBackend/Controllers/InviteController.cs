using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.Services;

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
            var adminEmail = User.Identity!.Name!;
            await _userService.InviteUsersAsync(dto.Emails, adminEmail);
            return Ok();
        }
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmInvite(ConfirmInviteDto dto)
        {
            await _userService.ConfirmInviteAsync(dto);
            return Ok();
        }
    }
}
