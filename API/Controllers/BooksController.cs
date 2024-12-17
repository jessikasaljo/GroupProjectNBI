using Application.Commands.AddBook;
using Application.Commands.RemoveBook;
using Application.Commands.UpdateBook;
using Application.DTOs;
using Application.Queries.GetAllBooks;
using Application.Queries.GetBookById;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator mediatr;

        public BooksController(IMediator mediatr)
        {
            this.mediatr = mediatr;
        }

        [HttpGet]
        [Route("GetBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await mediatr.Send(new GetAllBooksQuery()));
        }

        [HttpGet]
        [Route("GetBookById/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid book ID.");
            }

            var result = await mediatr.Send(new GetBookByIdQuery(id));
            if (result == null)
            {
                return NotFound($"No book found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] NewBookDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediatr.Send(new AddBookCommand(value));
            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid book ID.");
            }

            var result = await mediatr.Send(new UpdateBookByIdCommand(value, id));
            if (result == null)
            {
                return NotFound($"No book found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public async Task<IActionResult> DeleteBookById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid book ID.");
            }

            var result = await mediatr.Send(new DeleteBookByIdCommand(id));
            if (result == null)
            {
                return NotFound($"No book found with ID {id}.");
            }

            return Ok(result);
        }
    }
}
