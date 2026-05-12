using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Diet
{
    public sealed record GetDietSummaryQuery(string UserId) : IRequest<Result<object>>;

    public class GetDietSummaryQueryHandler : IRequestHandler<GetDietSummaryQuery, Result<object>>
    {
        private readonly IDietService _service;

        public GetDietSummaryQueryHandler(IDietService service)
        {
            _service = service;
        }


        public async Task<Result<object>> Handle(GetDietSummaryQuery request, CancellationToken cancellationToken)
        {
            // todo: pobrać dane z bazy
            

            var summary = new
            {
                CaloriesConsumed = 1800,
                CaloriesGoal = 2500,
                Protein = 120,
                Carbs = 200,
                Fat = 60
            };
            return Result<object>.Success(summary);
        }
    }
}
