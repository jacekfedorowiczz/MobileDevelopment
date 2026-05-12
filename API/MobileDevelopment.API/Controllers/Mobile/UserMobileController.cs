using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MobileDevelopment.API.Models.DTO.Auth;
using MobileDevelopment.API.Services.Commands.User;

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
                return BadRequest("Nie udało się pobrać id użytkownika");
            }

            return Ok(new { Message = "Udało się pobrać użytkownika z claimów.", UserId = userId });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Brak refresh tokenu.");
            }

            var command = new LogoutCommand(request.RefreshToken);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest("Coś poszło nie tak.");
            }

            return Ok(new { Message = "Wylogowano pomyślnie." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
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
