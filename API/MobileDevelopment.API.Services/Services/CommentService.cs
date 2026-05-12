using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces.Base;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;
using MobileDevelopment.API.Services.Model;
using MobileDevelopment.API.Services.Services.Base;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class CommentService : BaseService<Comment, CommentDto, CreateEditCommentDto>, ICommentService
    {
        public CommentService(IBaseEntityRepository<Comment> repository) : base(repository)
        {
        }

        public override Task<Result<int>> CreateAsync(CreateEditCommentDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<Result<IEnumerable<CommentDto>>> GetAllAsync(CancellationToken token = default)
        {
            var result = (await _repository.GetAllAsync(token)).Select(x => x.ToDto());
            return Result<IEnumerable<CommentDto>>.Success(result);
        }

        public override Task<Result<CommentDto>> GetByIdAsync(int id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<Result<PagedResult<CommentDto>>> GetPaginatedResultAsync(GetPagedQuery<CommentDto> query, CancellationToken ct)
        {
            var pagedEntities = await _repository.GetPagedDynamicAsync(
                searchValue: query.SearchValue,
                sortColumn: query.SortColumn,
                sortDirection: query.SortDirection,
                pageIndex: query.PageIndex,
                pageSize: query.PageSize,
                include: null,
                cancellationToken: ct
            );

            var dtos = pagedEntities.Items.Select(x => x.ToDto()).ToList();

            var result = new PagedResult<CommentDto>(
                items: dtos,
                count: pagedEntities.TotalCount,
                pageIndex: pagedEntities.PageIndex,
                pageSize: query.PageSize
            );

            return Result<PagedResult<CommentDto>>.Success(result);
        }

        public override Task<Result> UpdateAsync(int id, CreateEditCommentDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
