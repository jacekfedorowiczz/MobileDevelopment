using MediatR;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Queries.Post
{
    public sealed record GetAllPostsQuery(int PageNumber, int PageSize) : IRequest<Result<object>>;

    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, Result<object>>
    {
        public async Task<Result<object>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            // pobrać dane z bazy

            var posts = new List<object>
            {
                new {
                    id = "1",
                    user = new { name = "Anna Nowak", avatar = "https://i.pravatar.cc/150?u=anna" },
                    content = "Dzisiejszy trening pleców wszedł idealnie! Nowy rekord w martwym ciągu - 110kg! 💪🔥",
                    time = "2 godz. temu",
                    likes = 45,
                    comments = 12,
                    isLiked = true,
                    tags = new[] { "Trening", "MartwyCiąg", "Rekord" }
                },
                new {
                    id = "2",
                    user = new { name = "Piotr Wiśniewski", avatar = "https://i.pravatar.cc/150?u=piotr" },
                    content = "Szukam kogoś do wspólnych treningów na siłowni FitGym w centrum. Ktoś chętny? Trenuję zazwyczaj rano ok. 7:00.",
                    time = "5 godz. temu",
                    likes = 18,
                    comments = 4,
                    isLiked = false,
                    tags = new[] { "SzukamPartnera", "FitGym", "RannyTrening" }
                }
            };

            var pagedResponse = new
            {
                items = posts,
                pageNumber = request.PageNumber,
                totalPages = 1,
                totalCount = 2,
                hasPreviousPage = false,
                hasNextPage = false
            };

            return Result<object>.Success(pagedResponse);
        }
    }
}
