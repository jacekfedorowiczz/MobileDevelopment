using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Services.Interfaces.Base;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IProfileService : IBaseService<ProfileDto, CreateEditProfileDto>
    {
    }
}
