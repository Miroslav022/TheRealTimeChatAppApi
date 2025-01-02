using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;

public sealed record ConversationCommand(int createdBy, int userId) : ICommand
{
}
