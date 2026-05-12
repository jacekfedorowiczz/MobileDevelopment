using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet
{
    public sealed record RemoveWorkoutSetCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveWorkoutSetCommandHandler : IRequestHandler<RemoveWorkoutSetCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveWorkoutSetCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}