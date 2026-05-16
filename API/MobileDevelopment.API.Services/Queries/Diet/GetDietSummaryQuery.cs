using MediatR;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Diet
{
    public sealed record GetDietSummaryQuery(string UserId) : IRequest<Result<DietSummaryDto>>;

    public class GetDietSummaryQueryHandler : IRequestHandler<GetDietSummaryQuery, Result<DietSummaryDto>>
    {
        private readonly IDietService _dietService;

        public GetDietSummaryQueryHandler(IDietService dietService)
        {
            _dietService = dietService;
        }

        public async Task<Result<DietSummaryDto>> Handle(GetDietSummaryQuery request, CancellationToken cancellationToken)
        {
            if (!int.TryParse(request.UserId, out var userId))
            {
                return Result<DietSummaryDto>.Failure("Invalid user id.");
            }

            return await _dietService.GetSummaryForUserAsync(userId, cancellationToken);
        }
    }
}
