using MediatR;
using Microsoft.AspNetCore.Mvc;
using Real_Time_Chat_App.Abstraction;
using Real_Time_Chat_App.SharedKernel;
using RealTimeChatApp.Application.UseCases.FileUpload.Command;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Real_Time_Chat_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ApiController
    {
        public FileUploadController(ISender sender) : base(sender)
        {
        }

        // POST api/<FileUploadController>
        [HttpPost("upload")]
        public async Task<IActionResult> Post(IFormFile file, CancellationToken cancellationToken)
        {
            var command = new FileUploadCommand { File = file};
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Ok(result);
        }

    }
}
