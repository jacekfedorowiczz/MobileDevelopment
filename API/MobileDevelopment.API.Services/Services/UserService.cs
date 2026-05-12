using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Domain.Extensions;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Extensions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Commands.User;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repo, IPasswordHasher<User> passwordHasher, ILogger<UserService> logger)
        {
            _repo = repo;
            _passwordHasher = passwordHasher;
            _logger = logger;
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

            var user = await _repo.GetByEmailAsync(loginCmd.Email, cancellationToken);
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
                Login = cmd.Email,
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                MobilePhone = cmd.MobilePhone,
                DateOfBirth = cmd.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = string.Empty,
                Profile = new Profile
                {
                    Age = cmd.DateOfBirth.CalculateAge(),
                    Weight = 0M,
                    Height = 0M,
                    PreferredWeightUnit = WeightUnits.Kilos,
                    CurrentGoal = FitnessGoal.Maintain
                }
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, cmd.Password);
            var entity = await _repo.CreateAsync(user, token);
            return Result<User>.Success(entity);
        }

        public async Task<Result<bool>> RemoveUserAsync(int id, CancellationToken token = default)
        {
            var result = await _repo.DeleteAsync(id, token);
            return result ? Result<bool>.Success(true) : Result<bool>.Failure("Nie udało się usunąć użytkownika");
        }
    }
}
