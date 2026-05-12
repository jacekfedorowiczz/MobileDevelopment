using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.WorkoutSession
{
    public sealed record EditWorkoutSessionCommand(int Id, CreateEditWorkoutSessionDto Dto) : IRequest<Result<WorkoutSessionDto>>;

    public sealed class EditWorkoutSessionCommandValidator : AbstractValidator<EditWorkoutSessionCommand>
    {
        public EditWorkoutSessionCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditWorkoutSessionCommandHandler : IRequestHandler<EditWorkoutSessionCommand, Result<WorkoutSessionDto>>
    {
        public Task<Result<WorkoutSessionDto>> Handle(EditWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}