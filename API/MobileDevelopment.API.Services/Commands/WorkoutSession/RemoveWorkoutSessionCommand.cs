using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession
{
    public sealed record RemoveWorkoutSessionCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveWorkoutSessionCommandHandler : IRequestHandler<RemoveWorkoutSessionCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}