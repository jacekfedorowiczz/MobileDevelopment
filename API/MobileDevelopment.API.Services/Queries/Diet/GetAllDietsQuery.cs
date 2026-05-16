using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Diet
{
    public sealed record GetAllDietsQuery() : IRequest<Result<IEnumerable<DietDto>>>;

    public sealed class GetAllDietsQueryHandler : IRequestHandler<GetAllDietsQuery, Result<IEnumerable<DietDto>>>
    {
        private IDietService _service;
        private ILogger<GetAllDietsQueryHandler> _logger;

        public GetAllDietsQueryHandler(IDietService service, ILogger<GetAllDietsQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DietDto>>> Handle(GetAllDietsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetAllForCurrentUserAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while fetching diets: {Message}", e.Message);
                return Result<IEnumerable<DietDto>>.Failure(e.Message);
            }
        }
    }
}