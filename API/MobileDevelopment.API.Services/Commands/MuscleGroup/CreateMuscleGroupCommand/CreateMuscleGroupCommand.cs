using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.MuscleGroup.CreateMuscleGroupCommand
{
    public sealed record CreateMuscleGroupCommand(CreateEditMuscleGroupDto Dto) : IRequest<Result<MuscleGroupDto>>;

    public sealed class CreateMuscleGroupCommandValidator : AbstractValidator<CreateMuscleGroupCommand>
    {
        public CreateMuscleGroupCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name cannot be empty.");
        }
    }

    public sealed class CreateMuscleGroupCommandHandler : IRequestHandler<CreateMuscleGroupCommand, Result<MuscleGroupDto>>
    {
        private readonly IMuscleGroupService _service;

        public CreateMuscleGroupCommandHandler(IMuscleGroupService service)
        {
            _service = service;
        }

        public Task<Result<MuscleGroupDto>> Handle(CreateMuscleGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
