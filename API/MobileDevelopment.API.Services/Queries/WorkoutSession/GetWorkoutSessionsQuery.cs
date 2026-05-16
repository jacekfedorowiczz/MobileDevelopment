using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.WorkoutSession
{
    public sealed record GetWorkoutSessionsQuery() : IRequest<Result<IEnumerable<WorkoutSessionDto>>>;

    public sealed class GetWorkoutSessionsQueryHandler(IWorkoutSessionService workoutSessionService) : IRequestHandler<GetWorkoutSessionsQuery, Result<IEnumerable<WorkoutSessionDto>>>
    {
        public Task<Result<IEnumerable<WorkoutSessionDto>>> Handle(GetWorkoutSessionsQuery request, CancellationToken cancellationToken)
        {
            return workoutSessionService.GetAllSessionsForCurrentUserAsync(cancellationToken);
        }
    }
}