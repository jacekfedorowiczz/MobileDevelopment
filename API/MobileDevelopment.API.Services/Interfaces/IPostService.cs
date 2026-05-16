using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IPostService
    {
        Task<Result<PagedResult<PostDto>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
