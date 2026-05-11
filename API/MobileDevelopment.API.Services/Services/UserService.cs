using Microsoft.AspNetCore.Identity;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Extensions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Commands.User.LoginCommand;
using MobileDevelopment.API.Services.Commands.User.RegisterCommand;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IBaseEntityRepository<RefreshToken> _refreshTokenRepository;

        public UserService(IUserRepository repo, IPasswordHasher<User> passwordHasher, IBaseEntityRepository<RefreshToken> refreshTokenRepository)
        {
            _repo = repo;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task AddRefreshToken(int userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken!,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.CreateAsync(refreshTokenEntity, cancellationToken);
        }

        public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken token = default)
        {
            var user = await _repo.GetByIdAsync(id, token);
            if (user is null)
            {
                return Result<UserDto>.Failure("Cannot find the user with the specified id.");
            }

            return Result<UserDto>.Success(user.ToDto());
        }

        public async Task<Result<User>> ValidateCredentialsAsync(LoginCommand loginCmd, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(loginCmd);

            var user = await _repo.GetByEmailAsync(loginCmd.Email);
            if (user is null)
            {
                return Result<User>.Failure("Nieprawidłowy adres e-mail lub hasło.");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginCmd.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Result<User>.Failure("Nieprawidłowy adres e-mail lub hasło.");
            }

            return Result<User>.Success(user);
        }

        public async Task<Result<User>> RegisterUserAsync(RegisterCommand cmd, CancellationToken token = default)
        {
            var existingUser = await _repo.GetByEmailAsync(cmd.Email, token);
            if (existingUser is not null)
            {
                return Result<User>.Failure("Użytkownik o takim adresie e-mail już istnieje.");
            }

            var user = new User
            {
                Email = cmd.Email,
                Login = cmd.Email, // Używamy email jako login
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                MobilePhone = cmd.MobilePhone,
                DateOfBirth = cmd.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = string.Empty // Zostanie ustawione za chwilę
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, cmd.Password);

            await _repo.CreateAsync(user, token);
            return Result<User>.Success(user);
        }
    }
}
