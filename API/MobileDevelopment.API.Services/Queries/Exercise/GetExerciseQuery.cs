using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Exercise
{
    public sealed record GetExerciseQuery(int Id) : IRequest<Result<ExerciseDto>>;

    public sealed class GetExerciseQueryValidator : AbstractValidator<GetExerciseQuery>
    {
        public GetExerciseQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetExerciseQueryHandler(IExerciseService exerciseService) : IRequestHandler<GetExerciseQuery, Result<ExerciseDto>>
    {
        public Task<Result<ExerciseDto>> Handle(GetExerciseQuery request, CancellationToken cancellationToken)
        {
            return exerciseService.GetExerciseByIdAsync(request.Id, cancellationToken);
        }
    }
}