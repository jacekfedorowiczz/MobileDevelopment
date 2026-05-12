using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

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

    public sealed class CreateWorkoutSessionCommandHandler : IRequestHandler<CreateWorkoutSessionCommand, Result<WorkoutSessionDto>>
    {
        public Task<Result<WorkoutSessionDto>> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}