using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Base;
using Microsoft.AspNetCore.Http;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IProfileService : IBaseService<ProfileDto, CreateEditProfileDto>
    {
        Task<Result<MyProfileDto>> GetMyProfileAsync(CancellationToken ct = default);
        Task<Result> UpdateMyProfileAsync(CreateEditProfileDto dto, CancellationToken ct = default);
        Task<Result> UpdateDietAssumptionsAsync(CreateEditProfileDto dto, CancellationToken ct = default);
        Task<Result<string>> UpdateAvatarAsync(IFormFile file, CancellationToken ct = default);
    }
}
