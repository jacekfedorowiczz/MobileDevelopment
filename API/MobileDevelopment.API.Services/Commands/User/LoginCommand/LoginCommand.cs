using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MobileDevelopment.API.Models.DTO.Auth;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Services.Commands.User.LoginCommand
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
                .EmailAddress().WithMessage("Podano niepoprawny adres e-mail.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.");
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;

        public LoginCommandHandler(
            IUserService userService,
            ITokenService tokenService,
            IPasswordHasher<Domain.Entities.User> passwordHasher)
        {
            _userService = userService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.ValidateCredentialsAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                return Result<AuthResponse>.Failure(result.ErrorMessage!);
            }

            var user = result.Value!;
            if (user is null)
            {
                return Result<AuthResponse>.Failure("Nie można znaleźć użytkownika.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _userService.AddRefreshToken(user.Id, refreshToken, cancellationToken);

            var authResponse = new AuthResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(15)
            };

            return Result<AuthResponse>.Success(authResponse);
        }
    }
}
