using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.WorkoutSet
{
    public sealed record GetPagedWorkoutSetsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<WorkoutSetDto>>>;

    public sealed class GetPagedWorkoutSetsQueryValidator : AbstractValidator<GetPagedWorkoutSetsQuery>
    {
        public GetPagedWorkoutSetsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedWorkoutSetsQueryHandler : IRequestHandler<GetPagedWorkoutSetsQuery, Result<PagedResult<WorkoutSetDto>>>
    {
        public Task<Result<PagedResult<WorkoutSetDto>>> Handle(GetPagedWorkoutSetsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}