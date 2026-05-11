using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Base;
using MobileDevelopment.API.Services.Model;

namespace MobileDevelopment.API.Services.Services.Base
{
    public abstract class BaseService<TEntity, TDto, TCreateEditDto> : IBaseService<TDto, TCreateEditDto>
            where TEntity : BaseEntity
    {
        protected readonly IBaseEntityRepository<TEntity> _repository;

        protected BaseService(IBaseEntityRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public abstract Task<Result<TDto>> GetByIdAsync(int id, CancellationToken token = default);
        public abstract Task<Result<IEnumerable<TDto>>> GetAllAsync(CancellationToken token = default);
        public abstract Task<Result<int>> CreateAsync(TCreateEditDto dto, CancellationToken token = default);
        public abstract Task<Result> UpdateAsync(int id, TCreateEditDto dto, CancellationToken token = default);
        public abstract Task<Result<PagedResult<TDto>>> GetPaginatedResultAsync(GetPagedQuery<TDto> query, CancellationToken ct);

        public async Task<Result> DeleteAsync(int id, CancellationToken token = default)
        {
            try
            {
                await _repository.DeleteAsync(id, token);
                return Result.Success();

            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        public async Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken token = default)
        {
            try
            {
                await _repository.DeleteRangeAsync(ids, token);
                return Result.Success();

            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }
    }
}
