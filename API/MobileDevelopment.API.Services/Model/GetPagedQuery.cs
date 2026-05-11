using MediatR;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Model
{
    public record GetPagedQuery<TResponse>(
            int PageIndex = 1,
            int PageSize = 10,
            string? SearchValue = null,
            string? SortColumn = null,
            string? SortDirection = "asc"
        ) : IRequest<Result<PagedResult<TResponse>>>;
}
