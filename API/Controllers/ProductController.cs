using Application.Commands.AddAuthor;
using Application.Commands.DeleteAuthor;
using Application.Commands.UpdateProduct;
using Application.DTOs;
using Application.Queries.GetAllAuthors;
using Application.Queries.GetAuthorById;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediatr;

        public ProductsController(IMediator mediatr)
        {
            this.mediatr = mediatr;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await mediatr.Send(new GetAllProductsQuery()));
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var result = await mediatr.Send(new GetProductByIdQuery(id));
            if (result == null)
            {
                return NotFound($"No author found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "productAdmin")]
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediatr.Send(new AddProductCommand(value));
            return Ok(result);
        }

        [Authorize(Roles = "productAdmin")]
        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var result = await mediatr.Send(new UpdateProductByIdCommand(value, id));
            if (result == null)
            {
                return NotFound($"No author found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "productAdmin")]
        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var result = await mediatr.Send(new DeleteProductByIdCommand(id));
            if (result == null)
            {
                return NotFound($"No author found with ID {id}.");
            }

            return Ok(result);
        }
    }
}
