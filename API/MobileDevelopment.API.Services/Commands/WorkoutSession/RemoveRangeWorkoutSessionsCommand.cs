using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession
{
    public sealed record RemoveRangeWorkoutSessionsCommand(IEnumerable<int> Ids) : IRequest<Result>, IRemoveRangeCommand;

    public sealed class RemoveRangeWorkoutSessionsCommandHandler(IWorkoutSessionService workoutSessionService) : IRequestHandler<RemoveRangeWorkoutSessionsCommand, Result>
    {
        public Task<Result> Handle(RemoveRangeWorkoutSessionsCommand request, CancellationToken cancellationToken)
        {
            return workoutSessionService.DeleteSessionsRangeAsync(request.Ids, cancellationToken);
        }
    }
}