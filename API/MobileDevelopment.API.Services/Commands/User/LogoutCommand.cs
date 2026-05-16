using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.User
{
    public sealed record LogoutCommand(string RefreshToken) : IRequest<Result<bool>>;

    public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Brak refresh tokenu.");
        }
    }


    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<bool>>
    {
        private readonly ITokenService _service;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(ITokenService service, ILogger<LogoutCommandHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return Result<bool>.Success(await _service.RevokeTokenAsync(request.RefreshToken, cancellationToken));
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while revoking the refresh token: {Message}", e.Message);
                return Result<bool>.Failure(e.Message);
            }

        }
    }
}
