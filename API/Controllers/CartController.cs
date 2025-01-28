using Application.Commands.CartCommands.AddCart;
using Application.Commands.CartCommands.DeleteCart;
using Application.Commands.CartCommands.UpdateCart;
using Application.DTOs.CartDtos;
using Application.Queries.CartQuery.GetAllCarts;
using Application.Queries.CartQuery.GetCartById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetCarts")]
        public async Task<IActionResult> GetAllCarts([FromQuery] GetAllCartsQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet]
        [Route("GetCartById")]
        public async Task<IActionResult> GetCartById([FromHeader] int cartId)
        {
            if (cartId <= 0)
            {
                return BadRequest("Invalid cart ID.");
            }

            var result = await _mediator.Send(new GetCartByIdQuery(cartId));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }


        [HttpPost]
        [Route("AddCart")]
        public async Task<IActionResult> AddCart([FromBody] CartDTO newCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new AddCartCommand(newCart));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPut]
        [Route("UpdateCart/{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartDTO updatedCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid cart ID.");
            }

            var result = await _mediator.Send(new UpdateCartByIdCommand(id, updatedCart));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("DeleteCart/{id}")]
        public async Task<IActionResult> DeleteCartById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid cart ID.");
            }

            var result = await _mediator.Send(new DeleteCartByIdCommand(id));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            return Ok(result.Data);
        }
    }
}
