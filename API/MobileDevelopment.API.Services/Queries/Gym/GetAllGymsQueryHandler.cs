using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Gym
{
    public sealed class GetAllGymsQueryHandler : IRequestHandler<GetAllGymsQuery, Result<IEnumerable<GymDto>>>
    {
        private readonly IGymService _gymService;

        public GetAllGymsQueryHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result<IEnumerable<GymDto>>> Handle(GetAllGymsQuery request, CancellationToken cancellationToken)
        {
            return await _gymService.GetAllGymsAsync(request.Search);
        }
    }
}
