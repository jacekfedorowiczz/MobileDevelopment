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
    [MobileController("muscle-groups")]
    public class MuscleGroupsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MuscleGroupsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
