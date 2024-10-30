using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using RealTimeChatApp.Application.UseCases.Users.Commands.Login;
using RealTimeChatApp.Application.UseCases.Users.Commands.RegisterUser;
using RealTimeChatApp.Domain.Shared;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {

        public AuthController(ISender sender) : base(sender)
        {

        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationCommand command)
        {
            Result result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return HandleFailure(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginMember([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            Result<string> tokenResult = await Sender.Send(command, cancellationToken);
            if (tokenResult.IsFailure)
            {
                return HandleFailure(tokenResult);
            }
            return Ok(tokenResult.Value);
        }
    }
}
