using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet
{
    public sealed record RemoveRangeWorkoutSetsCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeWorkoutSetsCommandHandler : IRequestHandler<RemoveRangeWorkoutSetsCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeWorkoutSetsCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}