using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Diet
{
    public sealed record RemoveDietCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveDietCommandHandler : IRequestHandler<RemoveDietCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveDietCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}