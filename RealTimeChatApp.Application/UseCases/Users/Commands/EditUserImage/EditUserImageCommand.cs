using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.EditUserImage;

public sealed class EditUserImageCommand : ICommand
{
    public int Id { get; set; }
    public IFormFile File { get; set; }
}
