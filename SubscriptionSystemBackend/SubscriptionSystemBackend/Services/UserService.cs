using Microsoft.AspNetCore.Identity;
using SubscriptionSystemBackend.IRepositories;
using SubscriptionSystemBackend.IServices;
using SubscriptionSystemBackend.Models;
using SubscriptionSystemBackend.Repositories;

namespace SubscriptionSystemBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        public UserService(IUserRepository userRepository, IInvitationRepository invitationRepository, IAuthService authService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _invitationRepository = invitationRepository;
            _authService = authService; 
            _emailService = emailService;
        }
        public async Task RegisterAdminAsync(RegisterAdminDto dto)
        {
            if (dto.Token != "MOCK-ADMIN-TOKEN")
                throw new UnauthorizedAccessException("Invalid admin token");

            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = _authService.HashPassword(dto.Password),
                Role = "Admin",
                IsConfirmed = true
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !_authService.VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return _authService.GenerateJwtToken(user);
        }

        public async Task InviteUsersAsync(string[] emails, string adminEmail)
        {
            foreach (var email in emails)
            {
                var token = Guid.NewGuid().ToString();
                var invitation = new Invitation { Email = email, InvitationToken = token, IsUsed = false };
                await _invitationRepository.AddAsync(invitation);
                await _emailService.SendInvitationEmailASync(email, token);
            }
            await _invitationRepository.SaveChangesAsync();
        }

        public async Task ConfirmInviteAsync(ConfirmInviteDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var invite = await _invitationRepository.GetByTokenAsync(dto.Token);
            if (invite == null || invite.IsUsed)
                throw new Exception("Invalid or used token");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = invite.Email,
                PasswordHash = _authService.HashPassword((string)dto.Password),
                Role = "User",
                IsConfirmed = true
            };

            invite.IsUsed = true;
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

    }
}
