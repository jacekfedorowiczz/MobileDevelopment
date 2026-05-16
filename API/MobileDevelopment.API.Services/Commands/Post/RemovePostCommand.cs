using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Post
{
    public sealed record RemovePostCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemovePostCommandHandler : IRequestHandler<RemovePostCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemovePostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}