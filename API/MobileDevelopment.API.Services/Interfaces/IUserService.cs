using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken token = default);
    }
}
