using MediatR;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Queries.Calculators
{
    public sealed record CalculateYmcaBodyFatQuery(YmcaBodyFatRequestDto Dto) : IRequest<Result<YmcaBodyFatResultDto>>;

    public sealed class CalculateYmcaBodyFatQueryHandler(IHealthCalculatorFacade calculatorFacade)
        : IRequestHandler<CalculateYmcaBodyFatQuery, Result<YmcaBodyFatResultDto>>
    {
        public Task<Result<YmcaBodyFatResultDto>> Handle(CalculateYmcaBodyFatQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(Result<YmcaBodyFatResultDto>.Success(calculatorFacade.CalculateYmcaBodyFat(request.Dto)));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(Result<YmcaBodyFatResultDto>.Failure(ex.Message));
            }
        }
    }
}
