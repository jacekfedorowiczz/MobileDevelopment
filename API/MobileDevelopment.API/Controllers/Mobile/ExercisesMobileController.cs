using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Services.Commands.Exercise;
using MobileDevelopment.API.Services.Queries.Exercise;
using MobileDevelopment.API.Services.Queries.MuscleGroup;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("exercises")]
    public class ExercisesMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExercisesMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedExercises(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] string[]? muscleGroupIds = null,
            CancellationToken ct = default)
        {
            var query = new GetPagedExercisesQuery(pageNumber, pageSize, search, ParseMuscleGroupIds(muscleGroupIds));
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetExercise(int id, CancellationToken ct = default)
        {
            var query = new GetExerciseQuery(id);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpGet("muscle-groups")]
        public async Task<IActionResult> GetMuscleGroups(CancellationToken ct = default)
        {
            var query = new GetMuscleGroupsQuery();
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercise(
            [FromBody] MobileDevelopment.API.Models.DTO.Exercises.CreateEditExerciseDto dto,
            CancellationToken ct = default)
        {
            var command = new CreateExerciseCommand(dto);
            var result = await _mediator.Send(command, ct);
            return result.ToCreatedAtActionResult(this, nameof(GetExercise), new { id = result.Value?.Id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateExercise(
            int id,
            [FromBody] MobileDevelopment.API.Models.DTO.Exercises.CreateEditExerciseDto dto,
            CancellationToken ct = default)
        {
            var command = new EditExerciseCommand(id, dto);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteExercise(int id, CancellationToken ct = default)
        {
            var command = new RemoveExerciseCommand(id);
            var result = await _mediator.Send(command, ct);
            return result.ToNoContentResult(this);
        }

        private static IEnumerable<int>? ParseMuscleGroupIds(IEnumerable<string>? values)
        {
            var ids = values?
                .SelectMany(value => value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Select(value => int.TryParse(value, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToArray();

            return ids is { Length: > 0 } ? ids : null;
        }
    }
}
