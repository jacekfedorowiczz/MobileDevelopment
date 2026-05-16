using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Diet
{
    public sealed record CreateDietCommand(CreateEditDietDto Dto) : IRequest<Result<DietDto>>;

    public sealed class CreateDietCommandValidator : AbstractValidator<CreateDietCommand>
    {
        public CreateDietCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateDietCommandHandler(IDietService dietService) : IRequestHandler<CreateDietCommand, Result<DietDto>>
    {
        public Task<Result<DietDto>> Handle(CreateDietCommand request, CancellationToken cancellationToken)
        {
            return dietService.CreateAsync(request.Dto, cancellationToken);
        }
    }
}