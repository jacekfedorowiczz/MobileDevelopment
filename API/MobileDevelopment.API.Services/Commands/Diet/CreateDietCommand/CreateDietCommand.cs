using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Diet.CreateDietCommand
{
    public sealed record CreateDietCommand(CreateEditDietDto Dto) : IRequest<Result<DietDto>>;

    public sealed class CreateDietCommandValidator : AbstractValidator<CreateDietCommand>
    {
        public CreateDietCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Dto.StartDate).NotEmpty().WithMessage("StartDate cannot be empty.");
        }
    }

    public sealed class CreateDietCommandHandler : IRequestHandler<CreateDietCommand, Result<DietDto>>
    {
        private readonly IDietService _service;

        public CreateDietCommandHandler(IDietService service)
        {
            _service = service;
        }

        public Task<Result<DietDto>> Handle(CreateDietCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
