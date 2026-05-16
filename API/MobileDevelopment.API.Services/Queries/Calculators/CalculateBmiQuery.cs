using MediatR;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Queries.Calculators
{
    public sealed record CalculateBmiQuery(BmiRequestDto Dto) : IRequest<Result<BmiResultDto>>;

    public sealed class CalculateBmiQueryHandler(IHealthCalculatorFacade calculatorFacade)
        : IRequestHandler<CalculateBmiQuery, Result<BmiResultDto>>
    {
        public Task<Result<BmiResultDto>> Handle(CalculateBmiQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(Result<BmiResultDto>.Success(calculatorFacade.CalculateBmi(request.Dto)));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(Result<BmiResultDto>.Failure(ex.Message));
            }
        }
    }
}
