using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record RemoveExerciseCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveExerciseCommandHandler : IRequestHandler<RemoveExerciseCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveExerciseCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}