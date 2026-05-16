using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Queries.Gym
{
    public sealed record GetAllGymsQuery(string? Search = null) : IRequest<Result<IEnumerable<GymDto>>>;
}
