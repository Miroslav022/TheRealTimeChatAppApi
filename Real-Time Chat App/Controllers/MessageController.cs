using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Messages.Query;
using RealTimeChatApp.Domain.Shared;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
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
    }
}
