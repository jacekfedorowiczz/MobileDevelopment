using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.MuscleGroup
{
    public sealed record CreateMuscleGroupCommand(CreateEditMuscleGroupDto Dto) : IRequest<Result<MuscleGroupDto>>;

    public sealed class CreateMuscleGroupCommandValidator : AbstractValidator<CreateMuscleGroupCommand>
    {
        public CreateMuscleGroupCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateMuscleGroupCommandHandler : IRequestHandler<CreateMuscleGroupCommand, Result<MuscleGroupDto>>
    {
        public Task<Result<MuscleGroupDto>> Handle(CreateMuscleGroupCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}