using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Meal
{
    public sealed record GetMealsQuery() : IRequest<Result<IEnumerable<MealDto>>>;

    public sealed class GetMealsQueryHandler : IRequestHandler<GetMealsQuery, Result<IEnumerable<MealDto>>>
    {
        public Task<Result<IEnumerable<MealDto>>> Handle(GetMealsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}