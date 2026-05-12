using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Meal
{
    public sealed record RemoveMealCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveMealCommandHandler : IRequestHandler<RemoveMealCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveMealCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}