using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Gym
{
    public sealed class GetGymQueryHandler : IRequestHandler<GetGymQuery, Result<GymDto>>
    {
        private readonly IGymService _gymService;

        public GetGymQueryHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result<GymDto>> Handle(GetGymQuery request, CancellationToken cancellationToken)
        {
            return await _gymService.GetGymByIdAsync(request.Id);
        }
    }
}
