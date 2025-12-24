using CoreBanking.Application.Features.Accounts.Commands;
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
}