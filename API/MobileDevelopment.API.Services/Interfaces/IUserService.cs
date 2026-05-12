using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Commands.User;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken token = default);
        Task<Result<User>> ValidateCredentialsAsync(LoginCommand loginCmd, CancellationToken cancellationToken = default);
        Task<Result<User>> RegisterUserAsync(RegisterCommand cmd, CancellationToken token = default);
        Task<Result<bool>> RemoveUserAsync(int id, CancellationToken token = default);
    }
}
