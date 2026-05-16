using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession
{
    public sealed record RemoveWorkoutSessionCommand(int Id) : IRequest<Result>, IRemoveCommand;

    public sealed class RemoveWorkoutSessionCommandHandler(IWorkoutSessionService workoutSessionService) : IRequestHandler<RemoveWorkoutSessionCommand, Result>
    {
        public Task<Result> Handle(RemoveWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            return workoutSessionService.DeleteSessionAsync(request.Id, cancellationToken);
        }
    }
}