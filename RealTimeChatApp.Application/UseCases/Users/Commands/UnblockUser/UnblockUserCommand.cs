using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.UnblockUser;

public sealed record UnblockUserCommand(int userId, int blockedUserId) : ICommand
{
}
