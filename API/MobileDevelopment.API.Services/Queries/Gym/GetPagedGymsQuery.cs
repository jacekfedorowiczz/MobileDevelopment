using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Models.Pagination;

namespace MobileDevelopment.API.Services.Queries.Gym
{
    public sealed record GetPagedGymsQuery(int PageNumber, int PageSize) : IRequest<Result<PagedResult<GymDto>>>;
}
