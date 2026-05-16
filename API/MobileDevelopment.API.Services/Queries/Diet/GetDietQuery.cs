using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Diet
{
    public sealed record GetDietQuery(int Id) : IRequest<Result<DietDto>>;

    public sealed class GetDietQueryValidator : AbstractValidator<GetDietQuery>
    {
        public GetDietQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetDietQueryHandler : IRequestHandler<GetDietQuery, Result<DietDto>>
    {
        private IDietService _service;
        private ILogger<GetDietQueryHandler> _logger;

        public GetDietQueryHandler(IDietService service, ILogger<GetDietQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<DietDto>> Handle(GetDietQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetByIdAsync(request.Id, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while fetching diet: {Message}", e.Message);
                return Result<DietDto>.Failure(e.Message);
            }
        }
    }
}