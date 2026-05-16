using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Queries.Profile
{
    public sealed record GetMyProfileQuery() : IRequest<Result<MyProfileDto>>;
}
