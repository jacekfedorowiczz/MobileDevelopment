using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.WorkoutSession
{
    public sealed record GetPagedWorkoutSessionsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<WorkoutSessionSummaryDto>>>;

    public sealed class GetPagedWorkoutSessionsQueryValidator : AbstractValidator<GetPagedWorkoutSessionsQuery>
    {
        public GetPagedWorkoutSessionsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedWorkoutSessionsQueryHandler(IWorkoutSessionService workoutSessionService) : IRequestHandler<GetPagedWorkoutSessionsQuery, Result<PagedResult<WorkoutSessionSummaryDto>>>
    {
        public Task<Result<PagedResult<WorkoutSessionSummaryDto>>> Handle(GetPagedWorkoutSessionsQuery request, CancellationToken cancellationToken)
        {
            return workoutSessionService.GetPagedSessionsForCurrentUserAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}