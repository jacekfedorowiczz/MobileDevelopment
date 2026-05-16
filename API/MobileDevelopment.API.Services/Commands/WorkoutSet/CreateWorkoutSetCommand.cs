using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet
{
    public sealed record CreateWorkoutSetCommand(CreateEditWorkoutSetDto Dto) : IRequest<Result<WorkoutSetDto>>;

    public sealed class CreateWorkoutSetCommandValidator : AbstractValidator<CreateWorkoutSetCommand>
    {
        public CreateWorkoutSetCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateWorkoutSetCommandHandler(IWorkoutSetService workoutSetService) : IRequestHandler<CreateWorkoutSetCommand, Result<WorkoutSetDto>>
    {
        public Task<Result<WorkoutSetDto>> Handle(CreateWorkoutSetCommand request, CancellationToken cancellationToken)
        {
            return workoutSetService.CreateSetAsync(request.Dto, cancellationToken);
        }
    }
}