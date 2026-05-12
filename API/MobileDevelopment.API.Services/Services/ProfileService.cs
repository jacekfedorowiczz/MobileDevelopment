using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces.Base;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Model;
using MobileDevelopment.API.Services.Services.Base;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class ProfileService : BaseService<Profile, ProfileDto, CreateEditProfileDto>, IProfileService
    {
        private readonly ILogger<ProfileService> _service;

        public ProfileService(IBaseEntityRepository<Profile> repository, ILogger<ProfileService> service) : base(repository)
        {
            _service = service;
        }

        public override Task<Result<int>> CreateAsync(CreateEditProfileDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Result<IEnumerable<ProfileDto>>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Result<ProfileDto>> GetByIdAsync(int id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Result<PagedResult<ProfileDto>>> GetPaginatedResultAsync(GetPagedQuery<ProfileDto> query, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public override Task<Result> UpdateAsync(int id, CreateEditProfileDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
