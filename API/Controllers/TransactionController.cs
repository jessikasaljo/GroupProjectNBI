using Application.Commands.TransactionCommands.AddTransaction;
using Application.DTOs.TransactionDtos;
using Application.Queries.TransactionQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "storeAdmin")]
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
    }
}
