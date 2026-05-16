using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Meal
{
    public sealed record CreateMealCommand(CreateEditMealDto Dto) : IRequest<Result<MealDto>>;

    public sealed class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
    {
        public CreateMealCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateMealCommandHandler(IMealService mealService) : IRequestHandler<CreateMealCommand, Result<MealDto>>
    {
        public Task<Result<MealDto>> Handle(CreateMealCommand request, CancellationToken cancellationToken)
        {
            return mealService.CreateAsync(request.Dto, cancellationToken);
        }
    }
}