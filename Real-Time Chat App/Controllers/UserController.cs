using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.Users.Commands.CurrentUser;
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
    }
}
