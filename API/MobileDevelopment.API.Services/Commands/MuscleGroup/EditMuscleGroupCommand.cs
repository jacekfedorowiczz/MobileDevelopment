using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.MuscleGroup
{
    public sealed record EditMuscleGroupCommand(int Id, CreateEditMuscleGroupDto Dto) : IRequest<Result<MuscleGroupDto>>;

    public sealed class EditMuscleGroupCommandValidator : AbstractValidator<EditMuscleGroupCommand>
    {
        public EditMuscleGroupCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditMuscleGroupCommandHandler : IRequestHandler<EditMuscleGroupCommand, Result<MuscleGroupDto>>
    {
        public Task<Result<MuscleGroupDto>> Handle(EditMuscleGroupCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}