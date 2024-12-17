using Application.Commands.AddAuthor;
using Application.Commands.DeleteAuthor;
using Application.Commands.UpdateAuthor;
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
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator mediatr;

        public AuthorsController(IMediator mediatr)
        {
            this.mediatr = mediatr;
        }

        [HttpGet]
        [Route("GetAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await mediatr.Send(new GetAllAuthorsQuery()));
        }

        [HttpGet]
        [Route("GetAuthorById/{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var result = await mediatr.Send(new GetAuthorByIdQuery(id));
            if (result == null)
            {
                return NotFound($"No author found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("AddAuthor")]
        public async Task<IActionResult> AddAuthor([FromBody] NewAuthorDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediatr.Send(new AddAuthorCommand(value));
            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateAuthor/{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var result = await mediatr.Send(new UpdateAuthorByIdCommand(value, id));
            if (result == null)
            {
                return NotFound($"No author found with ID {id}.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthorById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var result = await mediatr.Send(new DeleteAuthorByIdCommand(id));
            if (result == null)
            {
                return NotFound($"No author found with ID {id}.");
            }

            return Ok(result);
        }
    }
}
