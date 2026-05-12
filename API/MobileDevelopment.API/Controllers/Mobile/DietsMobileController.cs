using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Services.Queries.Diet;
using System.Security.Claims;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("diets")]
    public sealed class DietsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DietsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDietSummary()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value 
                      ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Brak autoryzacji." });
            }

            var query = new GetDietSummaryQuery(userId);
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : Ok(result.Value);
        }
    }
}
