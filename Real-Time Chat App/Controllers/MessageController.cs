using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Messages.Command;
using RealTimeChatApp.Application.UseCases.Messages.Query;
using RealTimeChatApp.Domain.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ApiController
    {
        public MessageController(ISender sender) : base(sender)
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] MessageQuery query, CancellationToken cancellationToken) 
        {
            Result result = await Sender.Send(query);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, CancellationToken cancellationToken)
        {
            string token = Request.Cookies["access_token"];
            DeleteMessageCommand command = new (); 
            command.Token = token;
            command.Id = id;
            Result result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }
    }
}
