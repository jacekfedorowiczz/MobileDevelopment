using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet
{
    public sealed record RemoveWorkoutSetCommand(int Id) : IRequest<Result>, IRemoveCommand;

    public sealed class RemoveWorkoutSetCommandHandler(IWorkoutSetService workoutSetService) : IRequestHandler<RemoveWorkoutSetCommand, Result>
    {
        public Task<Result> Handle(RemoveWorkoutSetCommand request, CancellationToken cancellationToken)
        {
            return workoutSetService.DeleteSetAsync(request.Id, cancellationToken);
        }
    }
}