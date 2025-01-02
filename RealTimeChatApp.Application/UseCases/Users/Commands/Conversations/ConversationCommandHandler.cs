using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;

public class ConversationCommandHandler : ICommandHandler<ConversationCommand>
{
    private readonly IConversationRepository _conversationRepository;

    public ConversationCommandHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<Result> Handle(ConversationCommand request, CancellationToken cancellationToken)
    {
        Conversation conversation = new Conversation
        {
            CreatedBy = request.createdBy,
        };
        var participants = new List<ConversationParticipant>
        {
            new ConversationParticipant
            {
                Conversation = conversation,
                UserId = request.userId,
                IsAdmin = true,
            },
            new ConversationParticipant
            {
                Conversation = conversation,
                UserId = request.createdBy,
                IsAdmin = true,
            }
        };

        var result = await _conversationRepository.CreateConversation(conversation, participants, new List<int> { request.userId, request.createdBy }, cancellationToken);

        return result ? Result.Success() : Result.Failure(Error.Failure("Conversation.Failure", "Something went wrong during process"));
    }
}
