using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.MuscleGroup
{
    public sealed record GetPagedMuscleGroupsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<MuscleGroupDto>>>;

    public sealed class GetPagedMuscleGroupsQueryValidator : AbstractValidator<GetPagedMuscleGroupsQuery>
    {
        public GetPagedMuscleGroupsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedMuscleGroupsQueryHandler(IExerciseService exerciseService) : IRequestHandler<GetPagedMuscleGroupsQuery, Result<PagedResult<MuscleGroupDto>>>
    {
        public Task<Result<PagedResult<MuscleGroupDto>>> Handle(GetPagedMuscleGroupsQuery request, CancellationToken cancellationToken)
        {
            return exerciseService.GetPagedMuscleGroupsAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}