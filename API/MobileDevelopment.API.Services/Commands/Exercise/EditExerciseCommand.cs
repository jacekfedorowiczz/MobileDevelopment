using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record EditExerciseCommand(int Id, CreateEditExerciseDto Dto) : IRequest<Result<ExerciseDto>>;

    public sealed class EditExerciseCommandValidator : AbstractValidator<EditExerciseCommand>
    {
        public EditExerciseCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.Difficulty)
                .IsInEnum()
                .When(x => x.Dto.Difficulty.HasValue);
            RuleFor(x => x.Dto.ImageUrl)
                .MaximumLength(2048)
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.ImageUrl));
        }
    }

    public sealed class EditExerciseCommandHandler(IExerciseService exerciseService) : IRequestHandler<EditExerciseCommand, Result<ExerciseDto>>
    {
        public Task<Result<ExerciseDto>> Handle(EditExerciseCommand request, CancellationToken cancellationToken)
        {
            return exerciseService.UpdateExerciseAsync(request.Id, request.Dto, cancellationToken);
        }
    }
}