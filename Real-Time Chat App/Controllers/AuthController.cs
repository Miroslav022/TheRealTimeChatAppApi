using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Users.Commands.CurrentUser;
using RealTimeChatApp.Application.UseCases.Users.Commands.Login;
using RealTimeChatApp.Application.UseCases.Users.Commands.RefreshToken;
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
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginMember([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            Result<AuthenticationDto> tokenResult = await Sender.Send(command, cancellationToken);
            if (tokenResult.IsFailure)
            {
                return tokenResult.ToProblemDetails();
            }

            Response.Cookies.Append("refresh_token", tokenResult.Value.Refresh_token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            var tokenst = Response.Cookies;
            return Ok(tokenResult.Value.Access_token);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refresh_token");

            return Ok();
        }

        [HttpPost("refresh_token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refresh_token = Request.Cookies["refresh_token"];
            var command = new RefreshTokenCommand(refresh_token);
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return Unauthorized(
                new ProblemDetails
                {
                    Title = "Unauthorized",
                    Type = result.Error.Code,
                    Detail = result.Error.Description,
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            return Ok(result.Value);
        }

        [Authorize]
        [HttpGet("current_user")]
        public async Task<IActionResult> CurrentUser()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            string accessToken = null;
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                accessToken = authHeader.Substring("Bearer ".Length).Trim();
            }

            var command = new CurrentUserCommand(accessToken);
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }
    }
}