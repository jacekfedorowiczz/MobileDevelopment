using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Exercise
{
    public sealed record GetExercisesQuery() : IRequest<Result<IEnumerable<ExerciseDto>>>;

    public sealed class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, Result<IEnumerable<ExerciseDto>>>
    {
        public Task<Result<IEnumerable<ExerciseDto>>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}