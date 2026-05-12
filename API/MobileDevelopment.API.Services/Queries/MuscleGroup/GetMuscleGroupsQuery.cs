using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.MuscleGroup
{
    public sealed record GetMuscleGroupsQuery() : IRequest<Result<IEnumerable<MuscleGroupDto>>>;

    public sealed class GetMuscleGroupsQueryHandler : IRequestHandler<GetMuscleGroupsQuery, Result<IEnumerable<MuscleGroupDto>>>
    {
        public Task<Result<IEnumerable<MuscleGroupDto>>> Handle(GetMuscleGroupsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}