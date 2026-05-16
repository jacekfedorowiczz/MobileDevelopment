using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Queries.Gym
{
    public sealed record GetGymQuery(int Id) : IRequest<Result<GymDto>>;
}
