using RealTimeChatApp.Application.UseCases.Messages.DTOs;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.Abstractions.Repositories;

public interface IChatRepository
{
    Task SaveMessageAsync(Message message);
    Task<Result<IReadOnlyList<Message>>> GetMessages(int ConversationId);
}
