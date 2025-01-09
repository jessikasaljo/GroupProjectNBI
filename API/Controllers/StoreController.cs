using Application.Commands.StoreCommands.AddStore;
using Application.Commands.StoreCommands.DeleteStore;
using Application.Commands.StoreCommands.UpdateStore;
using Application.DTOs.StoreDtos;
using Application.Queries.StoreQueries.GetAllStores;
using Application.Queries.StoreQueries.GetStoreById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public StoreController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet]
        [Route("GetStores")]
        public async Task<IActionResult> GetAllStores([FromQuery] GetAllStoresQuery query)
        {
            var result = await _mediatr.Send(query);

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        [Route("GetStoreById/{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid store ID.");
            }

            var result = await _mediatr.Send(new GetStoreByIdQuery { Id = id });

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        //[Authorize(Roles = "storeAdmin")]
        [HttpPost]
        [Route("AddStore")]
        public async Task<IActionResult> AddStore([FromBody] AddStoreDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediatr.Send(new AddStoreCommand(value));

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        //[Authorize(Roles = "storeAdmin")]
        [HttpPut]
        [Route("UpdateStore/{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] Store value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid store ID.");
            }

            var result = await _mediatr.Send(new UpdateStoreByIdCommand(value, id));

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        //[Authorize(Roles = "storeAdmin")]
        [HttpDelete]
        [Route("DeleteStore/{id}")]
        public async Task<IActionResult> DeleteStoreById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid store ID.");
            }

            var result = await _mediatr.Send(new DeleteStoreByIdCommand(id));

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }
    }
}
