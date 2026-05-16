using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Gym
{
    public sealed class GetPagedGymsQueryHandler : IRequestHandler<GetPagedGymsQuery, Result<PagedResult<GymDto>>>
    {
        private readonly IGymService _gymService;

        public GetPagedGymsQueryHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result<PagedResult<GymDto>>> Handle(GetPagedGymsQuery request, CancellationToken cancellationToken)
        {
            return await _gymService.GetPagedGymsAsync(request.PageNumber, request.PageSize);
        }
    }
}
