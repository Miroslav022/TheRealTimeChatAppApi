using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using Real_Time_Chat_App.SignalR;
using RealTimeChatApp.Application.UseCases.Conversations.Commands;
using RealTimeChatApp.Application.UseCases.Conversations.Queries;
using RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;
using System.Security.Claims;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ApiController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public ConversationController(ISender sender, IHubContext<ChatHub> hubContext) : base(sender)
        {
            _hubContext = hubContext;
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("conversations")]
        public async Task<IActionResult> GetAllUserConversations(CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ConversationQuery command = new ConversationQuery( id : int.Parse(userId) ); // VALIDATE ID
            Result result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost("groupchat")]
        public async Task<IActionResult> CreateGroupChat([FromBody] CreateGroupChatCommand command, CancellationToken cancellationToken)
        {
            Result result = await Sender.Send(command);
            if(result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            //Send message to the front using signalR
            foreach (var userId in command.participantIds)
            {
                await _hubContext.Clients.User(userId.ToString())
               .SendAsync("GroupChatCreated", "newGroupChatCreated");
            }
            return Ok(result);
        }
    }
}
