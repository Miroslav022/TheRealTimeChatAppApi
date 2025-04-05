using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.BlockUser;

public sealed record BlockUserCommand(int userId, int blockedUserId) : ICommand
{
}
