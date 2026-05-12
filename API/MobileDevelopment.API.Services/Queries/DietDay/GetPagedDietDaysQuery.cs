using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.DietDay
{
    public sealed record GetPagedDietDaysQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<DietDayDto>>>;

    public sealed class GetPagedDietDaysQueryValidator : AbstractValidator<GetPagedDietDaysQuery>
    {
        public GetPagedDietDaysQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedDietDaysQueryHandler : IRequestHandler<GetPagedDietDaysQuery, Result<PagedResult<DietDayDto>>>
    {
        public Task<Result<PagedResult<DietDayDto>>> Handle(GetPagedDietDaysQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}