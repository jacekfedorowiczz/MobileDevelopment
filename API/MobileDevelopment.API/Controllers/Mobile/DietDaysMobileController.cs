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
    [MobileController("diet-days")]
    public sealed class DietDaysMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DietDaysMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
