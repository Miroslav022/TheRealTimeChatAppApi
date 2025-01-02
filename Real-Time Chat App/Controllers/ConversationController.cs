using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Conversations.Queries;
using RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;
using RealTimeChatApp.Domain.Shared;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ApiController
    {
        public ConversationController(ISender sender) : base(sender)
        {
        }

        [HttpPost("conversation")]
        public async Task<IActionResult> Conversation([FromBody] ConversationCommand command, CancellationToken cancellationToken)
        {
            Result result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetAllUserConversations([FromQuery] ConversationQuery command, CancellationToken cancellationToken)
        {
            Result result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }
    }
}
