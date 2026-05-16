using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.MuscleGroup
{
    public sealed record GetMuscleGroupsQuery() : IRequest<Result<IEnumerable<MuscleGroupDto>>>;

    public sealed class GetMuscleGroupsQueryHandler(IExerciseService exerciseService) : IRequestHandler<GetMuscleGroupsQuery, Result<IEnumerable<MuscleGroupDto>>>
    {
        public Task<Result<IEnumerable<MuscleGroupDto>>> Handle(GetMuscleGroupsQuery request, CancellationToken cancellationToken)
        {
            return exerciseService.GetAllMuscleGroupsAsync(cancellationToken);
        }
    }
}