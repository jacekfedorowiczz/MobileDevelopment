using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Services.Commands.WorkoutSession;
using MobileDevelopment.API.Services.Queries.WorkoutSession;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("workout-sessions")]
    public class WorkoutSessionsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkoutSessionsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedSessions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var query = new GetPagedWorkoutSessionsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSession(int id, CancellationToken ct = default)
        {
            var query = new GetWorkoutSessionQuery(id);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(
            [FromBody] MobileDevelopment.API.Models.DTO.WorkoutSessions.CreateEditWorkoutSessionDto dto,
            CancellationToken ct = default)
        {
            var command = new CreateWorkoutSessionCommand(dto);
            var result = await _mediator.Send(command, ct);
            return result.ToCreatedAtActionResult(this, nameof(GetSession), new { id = result.Value?.Id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSession(
            int id,
            [FromBody] MobileDevelopment.API.Models.DTO.WorkoutSessions.CreateEditWorkoutSessionDto dto,
            CancellationToken ct = default)
        {
            var command = new EditWorkoutSessionCommand(id, dto);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSession(int id, CancellationToken ct = default)
        {
            var command = new RemoveWorkoutSessionCommand(id);
            var result = await _mediator.Send(command, ct);
            return result.ToNoContentResult(this);
        }

        [HttpDelete("range")]
        public async Task<IActionResult> DeleteSessionsRange(
            [FromBody] IEnumerable<int> ids,
            CancellationToken ct = default)
        {
            var command = new RemoveRangeWorkoutSessionsCommand(ids);
            var result = await _mediator.Send(command, ct);
            return result.ToNoContentResult(this);
        }
    }
}
