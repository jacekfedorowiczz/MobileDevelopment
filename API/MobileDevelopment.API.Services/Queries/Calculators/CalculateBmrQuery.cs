using MediatR;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Queries.Calculators
{
    public sealed record CalculateBmrQuery(BmrRequestDto Dto) : IRequest<Result<BmrResultDto>>;

    public sealed class CalculateBmrQueryHandler(IHealthCalculatorFacade calculatorFacade)
        : IRequestHandler<CalculateBmrQuery, Result<BmrResultDto>>
    {
        public Task<Result<BmrResultDto>> Handle(CalculateBmrQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(Result<BmrResultDto>.Success(calculatorFacade.CalculateBmr(request.Dto)));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(Result<BmrResultDto>.Failure(ex.Message));
            }
        }
    }
}
