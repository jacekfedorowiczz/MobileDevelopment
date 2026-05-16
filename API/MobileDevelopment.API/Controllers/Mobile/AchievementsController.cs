using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Services.Queries.Achievement;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/mobile/achievements")]
    public class AchievementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AchievementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAchievements(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllAchievementsQuery(), ct);
            return result.ToActionResult(this);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyAchievements(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetMyAchievementsQuery(), ct);
            return result.ToActionResult(this);
        }
    }
}
