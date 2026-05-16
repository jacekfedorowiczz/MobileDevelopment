using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Meal
{
    public sealed record GetMealsQuery(int DietDayId) : IRequest<Result<IEnumerable<MealDto>>>;

    public sealed class GetMealsQueryHandler(IMealService mealService) : IRequestHandler<GetMealsQuery, Result<IEnumerable<MealDto>>>
    {
        public Task<Result<IEnumerable<MealDto>>> Handle(GetMealsQuery request, CancellationToken cancellationToken)
        {
            return mealService.GetAllByDietDayIdAsync(request.DietDayId, cancellationToken);
        }
    }
}