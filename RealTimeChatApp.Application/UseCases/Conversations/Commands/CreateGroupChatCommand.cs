using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Conversations.Commands;

public sealed record CreateGroupChatCommand(int createdById, IEnumerable<int> participantIds, string groupName) : ICommand
{
}
