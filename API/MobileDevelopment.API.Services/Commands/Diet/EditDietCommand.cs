using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Diet
{
    public sealed record EditDietCommand(int Id, CreateEditDietDto Dto) : IRequest<Result<DietDto>>;

    public sealed class EditDietCommandValidator : AbstractValidator<EditDietCommand>
    {
        public EditDietCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditDietCommandHandler(IDietService dietService) : IRequestHandler<EditDietCommand, Result<DietDto>>
    {
        public Task<Result<DietDto>> Handle(EditDietCommand request, CancellationToken cancellationToken)
        {
            return dietService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        }
    }
}