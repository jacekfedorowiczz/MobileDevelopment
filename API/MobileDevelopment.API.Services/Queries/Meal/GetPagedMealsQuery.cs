using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Meal
{
    public sealed record GetPagedMealsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<MealDto>>>;

    public sealed class GetPagedMealsQueryValidator : AbstractValidator<GetPagedMealsQuery>
    {
        public GetPagedMealsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedMealsQueryHandler : IRequestHandler<GetPagedMealsQuery, Result<PagedResult<MealDto>>>
    {
        public Task<Result<PagedResult<MealDto>>> Handle(GetPagedMealsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}