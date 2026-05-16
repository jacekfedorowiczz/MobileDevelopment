using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record RemoveExerciseCommand(int Id) : IRequest<Result>, IRemoveCommand;

    public sealed class RemoveExerciseCommandHandler(IExerciseService exerciseService) : IRequestHandler<RemoveExerciseCommand, Result>
    {
        public Task<Result> Handle(RemoveExerciseCommand request, CancellationToken cancellationToken)
        {
            return exerciseService.DeleteExerciseAsync(request.Id, cancellationToken);
        }
    }
}