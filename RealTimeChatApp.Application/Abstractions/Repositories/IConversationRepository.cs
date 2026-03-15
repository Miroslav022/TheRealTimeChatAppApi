using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Application.Abstractions.Repositories;

public interface IConversationRepository
{
    Task<Conversation> CreateConversation( int createdById, List<int> participantsIds, CancellationToken cancellationToken);
    Task<IReadOnlyList<ConversationDto>> GetAllUsersConversations(int id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Conversation>> getAllUserConversationsWithoutGroups(int id, CancellationToken cancellationToken = default);
    Task Insert(Conversation conversation, CancellationToken cancellationToken);
}
