using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record RemoveRangeExercisesCommand(IEnumerable<int> Ids) : IRequest<Result>, IRemoveRangeCommand;

    public sealed class RemoveRangeExercisesCommandHandler(IExerciseService exerciseService) : IRequestHandler<RemoveRangeExercisesCommand, Result>
    {
        public Task<Result> Handle(RemoveRangeExercisesCommand request, CancellationToken cancellationToken)
        {
            return exerciseService.DeleteExercisesRangeAsync(request.Ids, cancellationToken);
        }
    }
}