using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record CreateExerciseCommand(CreateEditExerciseDto Dto) : IRequest<Result<ExerciseDto>>;

    public sealed class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
    {
        public CreateExerciseCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Dto.MuscleGroupIds)
                .NotNull().WithMessage("MuscleGroupIds cannot be null.")
                .Must(ids => ids != null && ids.Any()).WithMessage("At least one muscle group is required.");
            RuleFor(x => x.Dto.Difficulty)
                .IsInEnum()
                .When(x => x.Dto.Difficulty.HasValue);
            RuleFor(x => x.Dto.ImageUrl)
                .MaximumLength(2048)
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.ImageUrl));
        }
    }

    public sealed class CreateExerciseCommandHandler(IExerciseService exerciseService) : IRequestHandler<CreateExerciseCommand, Result<ExerciseDto>>
    {
        public Task<Result<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            return exerciseService.CreateExerciseAsync(request.Dto, cancellationToken);
        }
    }
}