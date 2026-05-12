using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession
{
    public sealed record RemoveRangeWorkoutSessionsCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeWorkoutSessionsCommandHandler : IRequestHandler<RemoveRangeWorkoutSessionsCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeWorkoutSessionsCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}