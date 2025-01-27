using Application.Commands.TransactionCommands.AddTransaction;
using Application.DTOs.TransactionDtos;
using Application.Queries.TransactionQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public TransactionController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] GetAllTransactionsQuery query)
        {
            var result = await _mediatr.Send(query);

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        //[HttpGet]
        //[Route("GetTransactionById/{id}")]
        //public async Task<IActionResult> GetTransactionById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid transaction ID.");
        //    }

        //    var result = await _mediatr.Send(new GetTransactionByIdQuery { Id = id });

        //    if (result.Data != null)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return NotFound(result.ErrorMessage);
        //}

        [Authorize(Roles = "transactionAdmin")]
        [HttpPost]
        [Route("AddTransaction")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediatr.Send(new AddTransactionCommand(value));

            if (result.Data != null)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        //[Authorize(Roles = "transactionAdmin")]
        //[HttpPut]
        //[Route("UpdateTransaction/{id}")]
        //public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid transaction ID.");
        //    }

        //    var result = await _mediatr.Send(new UpdateTransactionByIdCommand(value, id));

        //    if (result.Data != null)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return NotFound(result.ErrorMessage);
        //}

        //[Authorize(Roles = "transactionAdmin")]
        //[HttpDelete]
        //[Route("DeleteTransaction/{id}")]
        //public async Task<IActionResult> DeleteTransactionById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid transaction ID.");
        //    }

        //    var result = await _mediatr.Send(new DeleteTransactionByIdCommand(id));

        //    if (result.Data != null)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return NotFound(result.ErrorMessage);
        //}
    }
}
