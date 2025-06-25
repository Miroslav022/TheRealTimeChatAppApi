using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Messages.Command;

public sealed class DeleteMessageCommand : ICommand
{
    public int Id { get; set; }
    public string Token { get; set; }
}
