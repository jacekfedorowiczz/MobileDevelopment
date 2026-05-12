using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Meal
{
    public sealed record EditMealCommand(int Id, CreateEditMealDto Dto) : IRequest<Result<MealDto>>;

    public sealed class EditMealCommandValidator : AbstractValidator<EditMealCommand>
    {
        public EditMealCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditMealCommandHandler : IRequestHandler<EditMealCommand, Result<MealDto>>
    {
        public Task<Result<MealDto>> Handle(EditMealCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}