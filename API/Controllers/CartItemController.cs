using Application.Commands.CartItemCommands.AddCartItem;
using Application.Commands.CartItemCommands.DeleteCartItem;
using Application.Commands.CartItemCommands.UpdateCartItem;
using Application.DTOs.CartItemDtos;
using Application.Queries.CartItemQueries.GetAllCartItems;
using Application.Queries.CartItemQueries.GetCartItemById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetCartItems")]
        public async Task<IActionResult> GetAllCartItems([FromHeader] int cartId)
        {
            if (cartId <= 0)
            {
                return BadRequest("Invalid cart ID.");
            }

            var result = await _mediator.Send(new GetAllCartItemsQuery(cartId));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet]
        [Route("GetCartItemById/{id}")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid cart item ID.");
            }

            var result = await _mediator.Send(new GetCartItemByIdQuery(id));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("AddCartItem")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemDTO newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new AddCartItemCommand(newItem));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPut]
        [Route("UpdateCartItem/{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] UpdateCartItemDTO updatedItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid cart item ID.");
            }

            var result = await _mediator.Send(new UpdateCartItemCommand(id, updatedItem));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("DeleteCartItem/{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid cart item ID.");
            }

            var result = await _mediator.Send(new DeleteCartItemCommand(id));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
