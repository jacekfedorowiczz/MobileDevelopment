using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Comment
{
    public sealed record RemoveCommentCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}