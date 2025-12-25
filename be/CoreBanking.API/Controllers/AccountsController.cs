using CoreBanking.Application.Features.Accounts.Commands;
using CoreBanking.Application.Features.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/Accounts
    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountCommand command)
    {
        // API Controller cực gọn, chỉ việc chuyển lệnh cho MediatR
        var accountNumber = await _mediator.Send(command);
        return Ok(new { AccountNumber = accountNumber });
    }
    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> GetByAccountNumber( string accountNumber)
    {
        try
        {
            var query = new GetAccountByNumberQuery(accountNumber);
            var accountDto = await _mediator.Send(query);
            return Ok(accountDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }

    }
}   