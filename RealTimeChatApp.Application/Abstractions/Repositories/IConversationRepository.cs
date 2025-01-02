using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Application.Abstractions.Repositories;

public interface IConversationRepository
{
    Task<bool> CreateConversation(Conversation conversation, List<ConversationParticipant> participants, List<int> ids, CancellationToken cancellationToken);
    Task<IReadOnlyList<ConversationDto>> GetAllUsersConversations(int id, CancellationToken cancellationToken);
}
