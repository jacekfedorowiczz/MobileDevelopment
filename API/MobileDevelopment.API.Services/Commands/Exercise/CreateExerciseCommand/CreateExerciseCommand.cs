using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Exercise.CreateExerciseCommand
{
    public sealed record CreateExerciseCommand(CreateEditExerciseDto Dto) : IRequest<Result<ExerciseDto>>;

    public sealed class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
    {
        public CreateExerciseCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Dto.MuscleGroupIds).NotNull().WithMessage("MuscleGroupIds cannot be null.");
        }
    }

    public sealed class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Result<ExerciseDto>>
    {
        private readonly IExerciseService _service;

        public CreateExerciseCommandHandler(IExerciseService service)
        {
            _service = service;
        }

        public Task<Result<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
