using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using System.Security.Claims;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Services.Queries.Profile;
using MobileDevelopment.API.Services.Commands.Profile;
using MobileDevelopment.API.Extensions;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("profile")]
    public sealed class ProfileMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value 
                      ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unauthorized or missing user id claim." });
            }

            var query = new GetMyProfileQuery();
            var result = await _mediator.Send(query);

            return result.ToActionResult(this);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] CreateEditProfileDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value 
                      ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unauthorized or missing user id claim." });
            }

            var command = new UpdateMyProfileCommand(dto);
            var result = await _mediator.Send(command);

            return result.ToActionResult(this);
        }

        [HttpPut("me/diet-assumptions")]
        public async Task<IActionResult> UpdateDietAssumptions([FromBody] CreateEditProfileDto dto)
        {
            var command = new UpdateDietAssumptionsCommand(dto);
            var result = await _mediator.Send(command);

            return result.ToActionResult(this);
        }

        [HttpPost("me/avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var command = new UpdateAvatarCommand(file);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }
    }
}
