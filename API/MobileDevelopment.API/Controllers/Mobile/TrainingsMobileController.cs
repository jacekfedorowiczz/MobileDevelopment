using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Services.Queries.Exercise;
using MobileDevelopment.API.Services.Queries.WorkoutSession;

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
            return result.ToActionResult(this);
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetWorkoutSessions()
        {
            var query = new GetWorkoutSessionsQuery();
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }
    }
}
