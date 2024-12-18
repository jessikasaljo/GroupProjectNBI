using Application.Commands.AddUser;
using Application.Commands.LoginUser;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediatr;

        public UserController(IMediator mediatr)
        {
            this.mediatr = mediatr;
        }

        //[HttpGet]
        //[Route("GetUsers")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    return Ok(await mediatr.Send(new GetAllUsersQuery()));
        //}

        //[HttpGet]
        //[Route("GetUserById/{id}")]
        //public async Task<IActionResult> GetUserById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid user ID.");
        //    }

        //    var result = await mediatr.Send(new GetUserByIdQuery(id));
        //    if (result == null)
        //    {
        //        return NotFound($"No user found with ID {id}.");
        //    }

        //    return Ok(result);
        //}

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediatr.Send(new AddUserCommand(value));
            return Ok(result);
        }

        [HttpPost]
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] UserDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediatr.Send(new LoginUserCommand(value));
            return Ok(result);
        }

        //[Authorize]
        //[HttpPut]
        //[Route("UpdateUser/{id}")]
        //public async Task<IActionResult> UpdateUser(int id, [FromBody] User value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid user ID.");
        //    }

        //    var result = await mediatr.Send(new UpdateUserByIdCommand(value, id));
        //    if (result == null)
        //    {
        //        return NotFound($"No user found with ID {id}.");
        //    }

        //    return Ok(result);
        //}

        //[Authorize]
        //[HttpDelete]
        //[Route("DeleteUser/{id}")]
        //public async Task<IActionResult> DeleteUserById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid user ID.");
        //    }

        //    var result = await mediatr.Send(new DeleteUserByIdCommand(id));
        //    if (result == null)
        //    {
        //        return NotFound($"No user found with ID {id}.");
        //    }

        //    return Ok(result);
        //}
    }
}
