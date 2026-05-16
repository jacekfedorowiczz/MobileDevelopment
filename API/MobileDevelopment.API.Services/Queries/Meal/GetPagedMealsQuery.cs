using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Meal
{
    public sealed record GetPagedMealsQuery(int DietDayId, int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<MealDto>>>;

    public sealed class GetPagedMealsQueryValidator : AbstractValidator<GetPagedMealsQuery>
    {
        public GetPagedMealsQueryValidator()
        {
            RuleFor(x => x.DietDayId).GreaterThan(0).WithMessage("DietDayId must be greater than 0.");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedMealsQueryHandler(IMealService mealService) : IRequestHandler<GetPagedMealsQuery, Result<PagedResult<MealDto>>>
    {
        public Task<Result<PagedResult<MealDto>>> Handle(GetPagedMealsQuery request, CancellationToken cancellationToken)
        {
            return mealService.GetPagedByDietDayIdAsync(request.DietDayId, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}