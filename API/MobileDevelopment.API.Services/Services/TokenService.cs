using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MobileDevelopment.API.Domain.Auth;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<TokenService> _logger;
        private readonly IRefreshTokenRepository _repository;

        public TokenService(IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger, IRefreshTokenRepository repository)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
            _repository = repository;
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

            await _repository.CreateAsync(refreshTokenEntity, cancellationToken);
        }

        public string GenerateAccessToken(User user)
        {
            _logger.LogDebug("Generating a new access token.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.MobilePhone, user.MobilePhone),
                new(ClaimTypes.Role, user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            _logger.LogDebug("Finished generating an access token.");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogError("Invalid JWT token detected.");
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }

        public async Task<bool> RevokeTokenAsync(string token, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var entity = await _repository.GetByTokenWithUserAsync(token, ct).ConfigureAwait(false);

            if (entity is null || entity.IsRevoked)
            {
                return true;
            }

            entity.RevokedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);
            return true;
        }
    }
}
