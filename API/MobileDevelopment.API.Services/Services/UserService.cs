using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken token = default)
        {
            var user = await _repo.GetByIdAsync(id, token);
            if (user is null)
            {
                return Result<UserDto>.Failure("Cannot find the user with the specified id.");
            }

            var dto = new UserDto(id, user.FirstName, user.LastName, user.Email, user.MobilePhone, user.CreatedAt, user.ProfileId);
            return Result<UserDto>.Success(dto);
        }
    }
}
