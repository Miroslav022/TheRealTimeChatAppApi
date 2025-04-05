using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.FileUpload.Command;

public sealed class FileUploadCommand : ICommand<string>
{
    public IFormFile File { get; set; }
}
