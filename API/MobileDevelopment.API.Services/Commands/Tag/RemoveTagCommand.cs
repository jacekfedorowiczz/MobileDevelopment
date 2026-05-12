using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Tag
{
    public sealed record RemoveTagCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveTagCommandHandler : IRequestHandler<RemoveTagCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveTagCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}