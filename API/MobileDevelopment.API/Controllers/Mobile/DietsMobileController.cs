using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Services.Commands.Diet;
using MobileDevelopment.API.Services.Queries.Diet;
using System.Security.Claims;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("diets")]
    public sealed class DietsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DietsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDietSummary()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value 
                      ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unauthorized." });
            }

            var query = new GetDietSummaryQuery(userId);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiets()
        {
            var query = new GetAllDietsQuery();
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedDiets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedDietsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDietById([FromRoute] int id)
        {
            var query = new GetDietQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiet([FromBody] CreateEditDietDto dto)
        {
            var command = new CreateDietCommand(dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDiet([FromRoute] int id, [FromBody] CreateEditDietDto dto)
        {
            var command = new EditDietCommand(id, dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDiet([FromRoute] int id)
        {
            var command = new RemoveDietCommand(id);
            var result = await _mediator.Send(command);
            return result.ToNoContentResult(this);
        }
    }
}
