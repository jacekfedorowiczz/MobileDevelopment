using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Auth;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.User.RegisterCommand
{
    public sealed record RegisterCommand(
        string Email, 
        string Password, 
        string FirstName, 
        string LastName, 
        string MobilePhone, 
        DateOnly DateOfBirth) : IRequest<Result<AuthResponse>>;

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Adres e-mail jest wymagany.")
                .EmailAddress().WithMessage("Podano niepoprawny adres e-mail.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.");
        }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.RegisterUserAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                return Result<AuthResponse>.Failure(result.ErrorMessage!);
            }

            var user = result.Value!;
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
