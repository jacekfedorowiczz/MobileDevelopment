using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession
{
    public sealed record CreateWorkoutSessionCommand(CreateEditWorkoutSessionDto Dto) : IRequest<Result<WorkoutSessionDto>>;

    public sealed class CreateWorkoutSessionCommandValidator : AbstractValidator<CreateWorkoutSessionCommand>
    {
        public CreateWorkoutSessionCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateWorkoutSessionCommandHandler(IWorkoutSessionService workoutSessionService) : IRequestHandler<CreateWorkoutSessionCommand, Result<WorkoutSessionDto>>
    {
        public Task<Result<WorkoutSessionDto>> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            return workoutSessionService.CreateSessionAsync(request.Dto, cancellationToken);
        }
    }
}