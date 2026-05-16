using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.PostLike
{
    public sealed record RemovePostLikeCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemovePostLikeCommandHandler : IRequestHandler<RemovePostLikeCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemovePostLikeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}