using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Conversations.Commands;

public sealed class CreateGroupChatCommandHandler : ICommandHandler<CreateGroupChatCommand>
{
    private readonly IConversationRepository _conversationRepository;

    public CreateGroupChatCommandHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<Result> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
    {
        Conversation groupConversation = new Conversation
        {
            CreatedBy = request.createdById,
            IsGroup = true,
            GroupName = request.groupName
        };

        var participants = request.participantIds.Select(x => new ConversationParticipant { UserId = x, IsAdmin = false }).ToList();
        participants.Add(new ConversationParticipant { UserId = request.createdById, IsAdmin = true });

        groupConversation.Participants = participants;

        await _conversationRepository.Insert(groupConversation, cancellationToken);

        return Result.Success();
    }
}
