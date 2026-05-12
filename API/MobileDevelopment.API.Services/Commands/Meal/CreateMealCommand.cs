using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

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

    public sealed class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, Result<MealDto>>
    {
        public Task<Result<MealDto>> Handle(CreateMealCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}