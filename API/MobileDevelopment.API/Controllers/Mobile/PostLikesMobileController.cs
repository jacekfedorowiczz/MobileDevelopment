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
    [MobileController("post-likes")]
    public class PostLikesMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostLikesMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
