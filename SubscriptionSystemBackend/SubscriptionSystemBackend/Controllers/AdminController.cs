using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Services;

namespace SubscriptionSystemBackend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController: Controller
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService) => _userService = userService;

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
