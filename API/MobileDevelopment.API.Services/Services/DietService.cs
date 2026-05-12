using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces.Base;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Model;
using MobileDevelopment.API.Services.Services.Base;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class DietService : BaseService<Diet, DietDto, CreateEditDietDto>, IDietService
    {
        private readonly ILogger<DietService> _logger;

        public DietService(IBaseEntityRepository<Diet> repository, ILogger<DietService> logger) : base(repository)
        {
            _logger = logger;
        }

        public override Task<Result<int>> CreateAsync(CreateEditDietDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Result<IEnumerable<DietDto>>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Result<DietDto>> GetByIdAsync(int id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Result<PagedResult<DietDto>>> GetPaginatedResultAsync(GetPagedQuery<DietDto> query, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public override Task<Result> UpdateAsync(int id, CreateEditDietDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
