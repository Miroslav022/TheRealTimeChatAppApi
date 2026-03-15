using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;

public sealed record ConversationCommand(int createdBy, int userId) : ICommand<ConversationDto>
{
}
