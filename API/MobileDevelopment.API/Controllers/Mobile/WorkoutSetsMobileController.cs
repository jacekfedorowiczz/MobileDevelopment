using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Services.Commands.WorkoutSet;
using MobileDevelopment.API.Services.Queries.WorkoutSet;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("workout-sets")]
    public class WorkoutSetsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkoutSetsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("by-session/{sessionId:int}")]
        public async Task<IActionResult> GetSetsBySession(int sessionId, CancellationToken ct = default)
        {
            var query = new GetWorkoutSetsQuery(sessionId);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSet(int id, CancellationToken ct = default)
        {
            var query = new GetWorkoutSetQuery(id);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSet(
            [FromBody] MobileDevelopment.API.Models.DTO.WorkoutSets.CreateEditWorkoutSetDto dto,
            CancellationToken ct = default)
        {
            var command = new CreateWorkoutSetCommand(dto);
            var result = await _mediator.Send(command, ct);
            return result.ToCreatedAtActionResult(this, nameof(GetSet), new { id = result.Value?.Id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSet(
            int id,
            [FromBody] MobileDevelopment.API.Models.DTO.WorkoutSets.CreateEditWorkoutSetDto dto,
            CancellationToken ct = default)
        {
            var command = new EditWorkoutSetCommand(id, dto);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSet(int id, CancellationToken ct = default)
        {
            var command = new RemoveWorkoutSetCommand(id);
            var result = await _mediator.Send(command, ct);
            return result.ToNoContentResult(this);
        }

        [HttpDelete("range")]
        public async Task<IActionResult> DeleteSetsRange(
            [FromBody] IEnumerable<int> ids,
            CancellationToken ct = default)
        {
            var command = new RemoveRangeWorkoutSetsCommand(ids);
            var result = await _mediator.Send(command, ct);
            return result.ToNoContentResult(this);
        }
    }
}
