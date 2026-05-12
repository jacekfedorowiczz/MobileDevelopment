using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Queries.Exercise;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("trainings")]
    public sealed class TrainingsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrainingsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("exercises")]
        public async Task<IActionResult> GetExercises()
        {
            var query = new GetExercisesQuery();
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetWorkoutSessions()
        {
            // Assuming GetWorkoutSessionsQuery exists or will be created
            return Ok(Result<List<object>>.Success(new List<object>()));
        }
    }
}
