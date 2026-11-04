using Microsoft.AspNetCore.Identity;
using SafetyVault.Api.Data;
using SafetyVault.Api.Entities;
using SafetyVault.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace SafetyVault.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TimeProvider _timeProvider;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, TimeProvider timeProvider, ITokenService tokenService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<RegisterUserResult> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == dto.UserName))
            {
                return new RegisterUserResult(RegisterUserStatus.UserNameAlreadyExists, null);
            }
            if (await _context.Users.AnyAsync(u => u.UserEmail == dto.UserEmail))
            {
                return new RegisterUserResult(RegisterUserStatus.UserEmailAlreadyExists, null);
            }
            var user = new User
            {
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                CreatedAt = _timeProvider.GetUtcNow()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                UserEmail = user.UserEmail
            };
            return new RegisterUserResult(RegisterUserStatus.Created, userDto);
        }

        public async Task<LoginUserResult> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.Login || u.UserEmail == dto.Login);

            if (user is null)
            {
                return new LoginUserResult(LoginUserStatus.InvalidCredentials, null);
            }

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verification == PasswordVerificationResult.Failed)
            {
                return new LoginUserResult(LoginUserStatus.InvalidCredentials, null);
            }

            var authResponse = _tokenService.GenerateToken(user);
            return new LoginUserResult(LoginUserStatus.Success, authResponse);
        }
    }
}