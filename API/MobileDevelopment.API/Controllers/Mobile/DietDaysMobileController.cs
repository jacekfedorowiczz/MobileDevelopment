using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Services.Commands.DietDay;
using MobileDevelopment.API.Services.Queries.DietDay;

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

        [HttpGet("by-diet/{dietId}")]
        public async Task<IActionResult> GetAllDietDays([FromRoute] int dietId)
        {
            var query = new GetDietDaysQuery(dietId);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("by-diet/{dietId}/paged")]
        public async Task<IActionResult> GetPagedDietDays([FromRoute] int dietId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedDietDaysQuery(dietId, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDietDayById([FromRoute] int id)
        {
            var query = new GetDietDayQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietDay([FromBody] CreateEditDietDayDto dto)
        {
            var command = new CreateDietDayCommand(dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDietDay([FromRoute] int id, [FromBody] CreateEditDietDayDto dto)
        {
            var command = new EditDietDayCommand(id, dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDietDay([FromRoute] int id)
        {
            var command = new RemoveDietDayCommand(id);
            var result = await _mediator.Send(command);
            return result.ToNoContentResult(this);
        }
    }
}
