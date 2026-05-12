using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Model;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Diet
{
    public sealed record GetPagedDietsQuery(GetPagedQuery<DietDto> Query) : IRequest<Result<PagedResult<DietDto>>>;

    public sealed class GetPagedDietsQueryValidator : AbstractValidator<GetPagedDietsQuery>
    {
        public GetPagedDietsQueryValidator()
        {
            RuleFor(x => x.Query.PageIndex).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.Query.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedDietsQueryHandler : IRequestHandler<GetPagedDietsQuery, Result<PagedResult<DietDto>>>
    {
        private IDietService _service;
        private ILogger<GetPagedDietsQueryHandler> _logger;

        public GetPagedDietsQueryHandler(IDietService service, ILogger<GetPagedDietsQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<PagedResult<DietDto>>> Handle(GetPagedDietsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetPaginatedResultAsync(request.Query, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Wystąpił błąd podczas pobierania zpaginowanych diet: {Message}", e.Message);
                return Result<PagedResult<DietDto>>.Failure(e.Message);
            }
        }
    }
}