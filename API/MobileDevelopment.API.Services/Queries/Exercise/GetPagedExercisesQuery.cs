using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Exercise
{
    public sealed record GetPagedExercisesQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? SearchPhrase = null,
        IEnumerable<int>? MuscleGroupIds = null) : IRequest<Result<PagedResult<ExerciseDto>>>;

    public sealed class GetPagedExercisesQueryValidator : AbstractValidator<GetPagedExercisesQuery>
    {
        public GetPagedExercisesQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedExercisesQueryHandler(IExerciseService exerciseService) : IRequestHandler<GetPagedExercisesQuery, Result<PagedResult<ExerciseDto>>>
    {
        public Task<Result<PagedResult<ExerciseDto>>> Handle(GetPagedExercisesQuery request, CancellationToken cancellationToken)
        {
            return exerciseService.GetPagedExercisesAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchPhrase,
                request.MuscleGroupIds,
                cancellationToken);
        }
    }
}