using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Meal.GetMealQuery
{
    public sealed record GetMealQuery(int DietDayId, int Id) : IRequest<Result<MealDto>>;

    public sealed class GetMealQueryValidator : AbstractValidator<GetMealQuery>
    {
        public GetMealQueryValidator()
        {
            RuleFor(x => x.DietDayId).GreaterThan(0).WithMessage("DietDayId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetMealQueryHandler : IRequestHandler<GetMealQuery, Result<MealDto>>
    {
        private readonly IMealService _service;

        public GetMealQueryHandler(IMealService service)
        {
            _service = service;
        }

        public Task<Result<MealDto>> Handle(GetMealQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
