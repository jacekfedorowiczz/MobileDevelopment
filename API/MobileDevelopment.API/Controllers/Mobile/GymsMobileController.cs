using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Services.Commands.Gym;
using MobileDevelopment.API.Services.Queries.Gym;
using Asp.Versioning;
using MobileDevelopment.API.Attributes;
using MobileDevelopment.API.Extensions;

namespace MobileDevelopment.API.Controllers.Mobile
{
    [ApiController]
    [ApiVersion("1.0")]
    [MobileController("gyms")]
    [Authorize]
    public sealed class GymsMobileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GymsMobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, CancellationToken ct = default)
        {
            var query = new GetAllGymsQuery(search);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedGymsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetGymQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEditGymDto dto)
        {
            var command = new CreateGymCommand(dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateEditGymDto dto)
        {
            var command = new EditGymCommand(id, dto);
            var result = await _mediator.Send(command);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new RemoveGymCommand(id);
            var result = await _mediator.Send(command);
            return result.ToNoContentResult(this);
        }

        [HttpDelete("range")]
        public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids)
        {
            var command = new RemoveRangeGymsCommand(ids);
            var result = await _mediator.Send(command);
            return result.ToNoContentResult(this);
        }
    }
}
