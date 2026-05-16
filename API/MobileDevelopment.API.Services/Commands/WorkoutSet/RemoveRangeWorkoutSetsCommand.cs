using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet
{
    public sealed record RemoveRangeWorkoutSetsCommand(IEnumerable<int> Ids) : IRequest<Result>, IRemoveRangeCommand;

    public sealed class RemoveRangeWorkoutSetsCommandHandler(IWorkoutSetService workoutSetService) : IRequestHandler<RemoveRangeWorkoutSetsCommand, Result>
    {
        public Task<Result> Handle(RemoveRangeWorkoutSetsCommand request, CancellationToken cancellationToken)
        {
            return workoutSetService.DeleteSetsRangeAsync(request.Ids, cancellationToken);
        }
    }
}