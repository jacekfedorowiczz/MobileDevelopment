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
    [MobileController("exercises")]
    public class ExercisesMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExercisesMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
