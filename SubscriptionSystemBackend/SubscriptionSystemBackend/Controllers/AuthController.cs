using Microsoft.AspNetCore.Mvc;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.Services;

namespace SubscriptionSystemBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService) => _userService = userService;
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDto dto)
        {
           await _userService.RegisterAdminAsync(dto);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _userService.LoginAsync(dto);
            return Ok(new { token });
        }
    }
}
