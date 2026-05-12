using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record RemoveRangeExercisesCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeExercisesCommandHandler : IRequestHandler<RemoveRangeExercisesCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeExercisesCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}