using MediatR;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Contacts.Queries;
using RealTimeChatApp.Domain.Shared;

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ApiController
    {
        public ContactController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Contacts([FromQuery] ContactsQuery query, CancellationToken cancellationToken) 
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
