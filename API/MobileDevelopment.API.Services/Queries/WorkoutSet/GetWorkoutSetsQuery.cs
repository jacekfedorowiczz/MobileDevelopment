using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.WorkoutSet
{
    public sealed record GetWorkoutSetsQuery() : IRequest<Result<IEnumerable<WorkoutSetDto>>>;

    public sealed class GetWorkoutSetsQueryHandler : IRequestHandler<GetWorkoutSetsQuery, Result<IEnumerable<WorkoutSetDto>>>
    {
        public Task<Result<IEnumerable<WorkoutSetDto>>> Handle(GetWorkoutSetsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}