using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MobileDevelopment.API.Models.DTO.Auth;
using MobileDevelopment.API.Services.Commands.User;
using MobileDevelopment.API.Extensions;

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

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "Could not resolve the current user id." });
            }

            return Ok(new { UserId = userId });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new { Message = "Refresh token is required." });
            }

            var command = new LogoutCommand(request.RefreshToken);
            var result = await _mediator.Send(command);

            return result.ToActionResult(this);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }
    }
}
