using Application.Commands.StoreItemCommands.AddStoreItem;
using Application.Commands.StoreItemCommands.DeleteStoreItem;
using Application.Commands.StoreItemCommands.UpdateStoreItem;
using Application.DTOs.StoreItemDtos;
using Application.Queries.StoreItemQueries.GetAllStoreItems;
using Application.Queries.StoreItemQueries.GetStoreItemById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoreItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetStoreItems")]
        public async Task<IActionResult> GetAllStoreItems([FromQuery] GetAllStoreItemsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("GetStoreItemById/{id}")]
        public async Task<IActionResult> GetStoreItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid store item ID.");
            }

            var result = await _mediator.Send(new GetStoreItemByIdQuery(id));
            if (result == null)
            {
                return NotFound($"No store item found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "storeAdmin")]
        [HttpPost]
        [Route("AddStoreItem")]
        public async Task<IActionResult> AddStoreItem([FromBody] AddStoreItemDTO storeItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new AddStoreItemCommand(storeItemDto));
            return Ok(result);
        }

        [Authorize(Roles = "storeAdmin")]
        [HttpPut]
        [Route("UpdateStoreItem/{id}")]
        public async Task<IActionResult> UpdateStoreItem(int id, [FromBody] UpdateStoreItemDTO storeItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid store item ID.");
            }

            var result = await _mediator.Send(new UpdateStoreItemCommand(storeItemDto, id));
            if (result == null)
            {
                return NotFound($"No store item found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "storeAdmin")]
        [HttpDelete]
        [Route("DeleteStoreItem/{id}")]
        public async Task<IActionResult> DeleteStoreItem(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid store item ID.");
            }

            var result = await _mediator.Send(new DeleteStoreItemByIdCommand(id));
            if (result == null)
            {
                return NotFound($"No store item found with ID {id}.");
            }

            return Ok(result);
        }
    }
}
