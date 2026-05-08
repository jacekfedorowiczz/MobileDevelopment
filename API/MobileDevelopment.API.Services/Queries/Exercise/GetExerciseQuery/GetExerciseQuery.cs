using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Exercise.GetExerciseQuery
{
    public sealed record GetExerciseQuery(int Id) : IRequest<Result<ExerciseDto>>;

    public sealed class GetExerciseQueryValidator : AbstractValidator<GetExerciseQuery>
    {
        public GetExerciseQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetExerciseQueryHandler : IRequestHandler<GetExerciseQuery, Result<ExerciseDto>>
    {
        private readonly IExerciseService _service;

        public GetExerciseQueryHandler(IExerciseService service)
        {
            _service = service;
        }

        public Task<Result<ExerciseDto>> Handle(GetExerciseQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
