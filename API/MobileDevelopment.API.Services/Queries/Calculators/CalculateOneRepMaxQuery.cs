using MediatR;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Queries.Calculators
{
    public sealed record CalculateOneRepMaxQuery(OneRepMaxRequestDto Dto) : IRequest<Result<OneRepMaxResultDto>>;

    public sealed class CalculateOneRepMaxQueryHandler(IHealthCalculatorFacade calculatorFacade)
        : IRequestHandler<CalculateOneRepMaxQuery, Result<OneRepMaxResultDto>>
    {
        public Task<Result<OneRepMaxResultDto>> Handle(CalculateOneRepMaxQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(Result<OneRepMaxResultDto>.Success(calculatorFacade.CalculateOneRepMax(request.Dto)));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(Result<OneRepMaxResultDto>.Failure(ex.Message));
            }
        }
    }
}
