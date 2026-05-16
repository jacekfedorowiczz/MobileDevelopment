using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.User
{
    public sealed record RemoveUserCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Result<bool>>
    {
        private readonly IUserService _service;
        private readonly ILogger<RemoveUserCommandHandler> _logger;

        public RemoveUserCommandHandler(IUserService service, ILogger<RemoveUserCommandHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.RemoveUserAsync(request.Id, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while deleting user: {Message}", e.Message);
                return Result<bool>.Failure(e.Message);
            }
        }
    }
}
