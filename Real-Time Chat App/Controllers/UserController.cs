using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Users.Commands.BlockUser;
using RealTimeChatApp.Application.UseCases.Users.Commands.CurrentUser;
using RealTimeChatApp.Application.UseCases.Users.Commands.EditUser;
using RealTimeChatApp.Application.UseCases.Users.Commands.EditUserImage;
using RealTimeChatApp.Application.UseCases.Users.Commands.UnblockUser;
using RealTimeChatApp.Application.UseCases.Users.Queries.FindUserByPhoneNumber;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiController
    {
        public UserController(ISender sender) : base(sender)
        {
        }

        [HttpGet("Search/{Phone}")]
        public async Task<IActionResult> GetUserByPhone(string Phone, CancellationToken cancellationToken)
        {
            var query = new FindUserByPhoneNumberQuery(Phone);
            var result = await Sender.Send(query);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result.Value);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserCommand command, int id, CancellationToken cancellationToken)
        {
            command.id = id;
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [HttpPost("editimage/{id}")]
        public async Task<IActionResult> EditUserProfileImage(IFormFile File, int id,  CancellationToken cancellationToken)
        {
            var command = new EditUserImageCommand { File = File, Id = id };
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserCommand command, CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockUser([FromBody] UnblockUserCommand command, CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }
    }
}
