using MediatR;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Queries.Calculators
{
    public sealed record CalculateIdealWeightQuery(IdealWeightRequestDto Dto) : IRequest<Result<IdealWeightResultDto>>;

    public sealed class CalculateIdealWeightQueryHandler(IHealthCalculatorFacade calculatorFacade)
        : IRequestHandler<CalculateIdealWeightQuery, Result<IdealWeightResultDto>>
    {
        public Task<Result<IdealWeightResultDto>> Handle(CalculateIdealWeightQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(Result<IdealWeightResultDto>.Success(calculatorFacade.CalculateIdealWeight(request.Dto)));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(Result<IdealWeightResultDto>.Failure(ex.Message));
            }
        }
    }
}
