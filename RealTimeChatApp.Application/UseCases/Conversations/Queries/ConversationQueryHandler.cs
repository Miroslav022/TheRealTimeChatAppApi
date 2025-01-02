using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Conversations.Queries;

public class ConversationQueryHandler : IQueryHandler<ConversationQuery, IReadOnlyList<ConversationDto>>
{
    private readonly IConversationRepository _conversationRepository;

    public ConversationQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<Result<IReadOnlyList<ConversationDto>>> Handle(ConversationQuery request, CancellationToken cancellationToken)
    {
        var conversations = await _conversationRepository.GetAllUsersConversations(request.id, cancellationToken);
        return Result.Success(conversations);
    }
}
