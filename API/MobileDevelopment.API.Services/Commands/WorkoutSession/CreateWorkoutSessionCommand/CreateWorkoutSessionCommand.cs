using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession.CreateWorkoutSessionCommand
{
    public sealed record CreateWorkoutSessionCommand(CreateEditWorkoutSessionDto Dto) : IRequest<Result<WorkoutSessionDto>>;

    public sealed class CreateWorkoutSessionCommandValidator : AbstractValidator<CreateWorkoutSessionCommand>
    {
        public CreateWorkoutSessionCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Dto.StartTime).NotEmpty().WithMessage("StartTime cannot be empty.");
        }
    }

    public sealed class CreateWorkoutSessionCommandHandler : IRequestHandler<CreateWorkoutSessionCommand, Result<WorkoutSessionDto>>
    {
        private readonly IWorkoutSessionService _service;

        public CreateWorkoutSessionCommandHandler(IWorkoutSessionService service)
        {
            _service = service;
        }

        public Task<Result<WorkoutSessionDto>> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
