using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using System.Security.Claims;

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
                return Unauthorized(new { Message = "Brak autoryzacji lub id użytkownika w tokenie." });
            }

            // TODO: Zaimplementuj pobieranie profilu za pomocą Mediator (np. GetUserProfileQuery)
            // var query = new GetUserProfileQuery(userId);
            // var result = await _mediator.Send(query);
            // return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);

            // Tymczasowy mock dla frontend'u
            return Ok(new
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@email.com",
                WorkoutsThisMonth = 24,
                AverageWorkoutTime = 48,
                AchievementsCount = 12
            });
        }
    }
}
