using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Services.Commands.Meal;
using MobileDevelopment.API.Services.Queries.Meal;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("meals")]
    public sealed class MealsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MealsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("by-diet-day/{dietDayId}")]
        public async Task<IActionResult> GetAllMeals([FromRoute] int dietDayId)
        {
            var query = new GetMealsQuery(dietDayId);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("by-diet-day/{dietDayId}/paged")]
        public async Task<IActionResult> GetPagedMeals([FromRoute] int dietDayId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedMealsQuery(dietDayId, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMealById([FromRoute] int id)
        {
            var query = new GetMealQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal([FromBody] CreateEditMealDto dto)
        {
            var command = new CreateMealCommand(dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMeal([FromRoute] int id, [FromBody] CreateEditMealDto dto)
        {
            var command = new EditMealCommand(id, dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMeal([FromRoute] int id)
        {
            var command = new RemoveMealCommand(id);
            var result = await _mediator.Send(command);
            return result.ToNoContentResult(this);
        }
    }
}
