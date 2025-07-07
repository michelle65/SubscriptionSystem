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
        private readonly IMockAdminService _mockAdminService;

        public UserService(
            IUserRepository userRepository, 
            IInvitationRepository invitationRepository, 
            IAuthService authService, 
            IEmailService emailService,
            IMockAdminService mockAdminService)
        {
            _userRepository = userRepository;
            _invitationRepository = invitationRepository;
            _authService = authService; 
            _emailService = emailService;
            _mockAdminService = mockAdminService;
        }

        public async Task RegisterAdminAsync(RegisterAdminDto dto)
        {
            var isTokenUsed = await _mockAdminService.IsTokenUsedAsync(dto.Token);
            if (isTokenUsed)
            {
                throw new UnauthorizedAccessException("This admin token has already been used for registration. Only one administrator can be registered with this token.");
            }

            var mockAdmin = await _mockAdminService.ValidateTokenAsync(dto.Token);
            if (mockAdmin == null)
                throw new UnauthorizedAccessException($"Invalid admin token. Available tokens: {string.Join(", ", await _mockAdminService.GetAvailableTokensAsync())}");

            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = _authService.HashPassword(dto.Password),
                Role = "Admin",
                IsConfirmed = true,
                FiscalCode = dto.FiscalCode ?? string.Empty
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            await _mockAdminService.MarkTokenAsUsedAsync(dto.Token);

            await _mockAdminService.LogAdminActivityAsync(dto.Token, $"Registered new admin: {dto.Email}");
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !_authService.VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _authService.GenerateJwtToken(user);
            
            return new LoginResult
            {
                Token = token,
                User = new UserInfo
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    HasFiscalCode = !string.IsNullOrEmpty(user.FiscalCode) && user.FiscalCode.Length == 16
                }
            };
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

        public async Task<IEnumerable<Invitation>> GetAllInvitationsAsync()
        {
            return await _invitationRepository.GetAllAsync();
        }

        public async Task UpdateFiscalCodeAsync(string email, string fiscalCode)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                var allUsers = await _userRepository.GetAllUsersAsync();
                var userEmails = string.Join(", ", allUsers.Select(u => u.Email));
                throw new Exception($"User not found with email: {email}. Available users: {userEmails}");
            }

            if (user.Role != "Admin")
                throw new UnauthorizedAccessException("Only administrators can update fiscal codes");

            if (string.IsNullOrEmpty(fiscalCode) || fiscalCode.Length != 16)
                throw new Exception("Fiscal code must be 16 characters long");

            user.FiscalCode = fiscalCode;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> CheckAdminFiscalCodeAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return false;

            if (user.Role != "Admin")
                return false;

            return !string.IsNullOrEmpty(user.FiscalCode) && user.FiscalCode.Length == 16;
        }
    }
}
