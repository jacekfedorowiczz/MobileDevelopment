using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;

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
    }
}
