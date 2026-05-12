using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.WorkoutSet
{
    public sealed record EditWorkoutSetCommand(int Id, CreateEditWorkoutSetDto Dto) : IRequest<Result<WorkoutSetDto>>;

    public sealed class EditWorkoutSetCommandValidator : AbstractValidator<EditWorkoutSetCommand>
    {
        public EditWorkoutSetCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditWorkoutSetCommandHandler : IRequestHandler<EditWorkoutSetCommand, Result<WorkoutSetDto>>
    {
        public Task<Result<WorkoutSetDto>> Handle(EditWorkoutSetCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}