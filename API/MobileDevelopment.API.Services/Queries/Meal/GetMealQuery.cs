using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Meal
{
    public sealed record GetMealQuery(int Id) : IRequest<Result<MealDto>>;

    public sealed class GetMealQueryValidator : AbstractValidator<GetMealQuery>
    {
        public GetMealQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetMealQueryHandler : IRequestHandler<GetMealQuery, Result<MealDto>>
    {
        public Task<Result<MealDto>> Handle(GetMealQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}