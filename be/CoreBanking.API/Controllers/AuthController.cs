using CoreBanking.Application.Features.Auths.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        // Add your authentication methods here
        public AuthController(IMediator mediator)
        {

            _mediator = mediator;
        }
        [HttpPost]
        public IActionResult Login(LoginCommand loginCommand)
        {
            var result = _mediator.Send(loginCommand);
            return Ok(result);

        }
    }
}
