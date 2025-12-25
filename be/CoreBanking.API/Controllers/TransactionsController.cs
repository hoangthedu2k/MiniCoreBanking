using CoreBanking.Application.Features.Transactions.Commands;
using CoreBanking.Application.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/Transactions/transfer
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(TransferMoneyCommand command)
    {
        try
        {
            var transactionId = await _mediator.Send(command);
            return Ok(new { TransactionId = transactionId, Message = "Chuyển tiền thành công!" });
        }
        catch (Exception ex)
        {
            // Nếu lỗi (ví dụ không đủ tiền), trả về Bad Request
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpGet("history/{accountNumber}")]
    public async Task<IActionResult> GetTransactionHistory(string accountNumber)
    {
        try
        {
            // Giả sử bạn có một Query để lấy lịch sử giao dịch
            var query = new GetTransactionsHistoryQuery(accountNumber);
            var transactions = await _mediator.Send(query);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }


    }
}