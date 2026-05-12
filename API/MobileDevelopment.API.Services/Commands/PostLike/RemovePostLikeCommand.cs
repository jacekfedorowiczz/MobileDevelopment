using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.PostLike
{
    public sealed record RemovePostLikeCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemovePostLikeCommandHandler : IRequestHandler<RemovePostLikeCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemovePostLikeCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}