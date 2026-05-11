using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Services.Commands.User.LoginCommand;
using MobileDevelopment.API.Services.Commands.User.RegisterCommand;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("user")]
    public sealed class UserMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new { Message = "Hello from Mobile API!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
