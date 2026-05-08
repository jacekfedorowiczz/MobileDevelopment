using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Meal.CreateMealCommand
{
    public sealed record CreateMealCommand(CreateEditMealDto Dto) : IRequest<Result<MealDto>>;

    public sealed class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
    {
        public CreateMealCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.DietDayId).GreaterThan(0).WithMessage("DietDayId must be greater than 0.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Dto.TotalCalories).GreaterThanOrEqualTo(0).WithMessage("TotalCalories must be 0 or greater.");
            RuleFor(x => x.Dto.Protein).GreaterThanOrEqualTo(0).WithMessage("Protein must be 0 or greater.");
            RuleFor(x => x.Dto.Carbs).GreaterThanOrEqualTo(0).WithMessage("Carbs must be 0 or greater.");
            RuleFor(x => x.Dto.Fats).GreaterThanOrEqualTo(0).WithMessage("Fats must be 0 or greater.");
        }
    }

    public sealed class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, Result<MealDto>>
    {
        private readonly IMealService _service;

        public CreateMealCommandHandler(IMealService service)
        {
            _service = service;
        }

        public Task<Result<MealDto>> Handle(CreateMealCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
