using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Result<PagedResult<PostDto>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var normalizedPageNumber = Math.Max(1, pageNumber);
            var normalizedPageSize = Math.Clamp(pageSize, 1, 50);

            var query = _postRepository.GetQueryable()
                .Include(post => post.User)
                .Include(post => post.Comments)
                .Include(post => post.Likes)
                .Include(post => post.Tags)
                .OrderByDescending(post => post.CreatedAt);

            var totalCount = await query.CountAsync(ct);
            var posts = await query
                .Skip((normalizedPageNumber - 1) * normalizedPageSize)
                .Take(normalizedPageSize)
                .ToListAsync(ct);

            var result = new PagedResult<PostDto>(
                posts.Select(post => post.ToDto(includeUser: true, includeComments: true, includeLikes: true, includeTags: true)).ToList(),
                totalCount,
                normalizedPageNumber,
                normalizedPageSize);

            return Result<PagedResult<PostDto>>.Success(result);
        }
    }
}
