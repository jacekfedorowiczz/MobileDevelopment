using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Services.Queries.Calculators;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("calculators")]
    public sealed class CalculatorsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalculatorsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("bmi")]
        public async Task<IActionResult> CalculateBmi([FromBody] BmiRequestDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CalculateBmiQuery(dto), ct);
            return result.ToActionResult(this);
        }

        [HttpPost("one-rep-max")]
        public async Task<IActionResult> CalculateOneRepMax([FromBody] OneRepMaxRequestDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CalculateOneRepMaxQuery(dto), ct);
            return result.ToActionResult(this);
        }

        [HttpPost("bmr")]
        public async Task<IActionResult> CalculateBmr([FromBody] BmrRequestDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CalculateBmrQuery(dto), ct);
            return result.ToActionResult(this);
        }

        [HttpPost("body-fat/ymca")]
        public async Task<IActionResult> CalculateYmcaBodyFat([FromBody] YmcaBodyFatRequestDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CalculateYmcaBodyFatQuery(dto), ct);
            return result.ToActionResult(this);
        }

        [HttpPost("ideal-weight")]
        public async Task<IActionResult> CalculateIdealWeight([FromBody] IdealWeightRequestDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CalculateIdealWeightQuery(dto), ct);
            return result.ToActionResult(this);
        }
    }
}
