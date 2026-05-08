using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet.CreateWorkoutSetCommand
{
    public sealed record CreateWorkoutSetCommand(CreateEditWorkoutSetDto Dto) : IRequest<Result<WorkoutSetDto>>;

    public sealed class CreateWorkoutSetCommandValidator : AbstractValidator<CreateWorkoutSetCommand>
    {
        public CreateWorkoutSetCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.WorkoutSessionId).GreaterThan(0).WithMessage("WorkoutSessionId must be greater than 0.");
            RuleFor(x => x.Dto.ExerciseId).GreaterThan(0).WithMessage("ExerciseId must be greater than 0.");
            RuleFor(x => x.Dto.SetNumber).GreaterThan(0).WithMessage("SetNumber must be greater than 0.");
            RuleFor(x => x.Dto.Weight).GreaterThanOrEqualTo(0).WithMessage("Weight must be 0 or greater.");
            RuleFor(x => x.Dto.Reps).GreaterThan(0).WithMessage("Reps must be greater than 0.");
        }
    }

    public sealed class CreateWorkoutSetCommandHandler : IRequestHandler<CreateWorkoutSetCommand, Result<WorkoutSetDto>>
    {
        private readonly IWorkoutSetService _service;

        public CreateWorkoutSetCommandHandler(IWorkoutSetService service)
        {
            _service = service;
        }

        public Task<Result<WorkoutSetDto>> Handle(CreateWorkoutSetCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
