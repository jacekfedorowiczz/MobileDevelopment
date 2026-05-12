using MediatR;
using MobileDevelopment.API.Models.DTO.Auth;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.User
{
    public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResponse>>;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenWithUserAsync(request.RefreshToken, cancellationToken);

            if (storedToken == null || !storedToken.IsActive)
            {
                return Result<AuthResponse>.Failure("Nieprawidłowy lub wygasły refresh token.");
            }

            var user = storedToken.User;
            if (user == null)
            {
                return Result<AuthResponse>.Failure("Nie znaleziono użytkownika dla tego tokenu.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            storedToken.Token = newRefreshToken;
            storedToken.ExpiresAt = DateTime.UtcNow.AddDays(7);
            
            await _refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync();

            var authResponse = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(15)
            };

            return Result<AuthResponse>.Success(authResponse);
        }
    }
}
