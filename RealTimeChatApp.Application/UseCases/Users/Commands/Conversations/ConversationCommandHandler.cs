using FluentValidation.Validators;
using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Conversations;

public class ConversationCommandHandler : ICommandHandler<ConversationCommand, ConversationDto>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IContactRepository _contactRepository;
    public ConversationCommandHandler(IConversationRepository conversationRepository, IContactRepository contactRepository)
    {
        _conversationRepository = conversationRepository;
        _contactRepository = contactRepository;
    }

    public async Task<Result<ConversationDto>> Handle(ConversationCommand request, CancellationToken cancellationToken)
    {

        var result = await _conversationRepository.CreateConversation(request.createdBy, new List<int> { request.userId, request.createdBy }, cancellationToken);

        if(result is null) Result.Failure<Conversation>(Error.Failure("Conversation.Failure", "Something went wrong during process"));

        _contactRepository.CreateContact(request.createdBy, request.userId);

        var conversation = new
        {
            result.Id,
            result.GroupName,
            result.IsGroup,
            result.LastMessageAt,
            LastMessage = result.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault(m => !m.IsDeleted),
            Participants = result.Participants.Where(participant => participant.UserId != request.createdBy).Select(participant => new ParticipantsDto
            (
                participant.UserId,
                participant.User.Username,
                participant.User.ProfilePicture,
                result.CreatedByUser != null && result.CreatedByUser.BlockedContacts.Any(bc => bc.BlockedUserId == participant.UserId)
            )).ToList()
        };


        var displayName = conversation.IsGroup ? conversation.GroupName : conversation.Participants?.FirstOrDefault()?.userName;
        var displayPicture = conversation.IsGroup ? "groupDefault.png" : conversation.Participants?.FirstOrDefault()?.profilePicture;

        var conversationResult = new ConversationDto
        (
            conversation.Id,
            displayName,
            displayPicture,
            conversation.IsGroup,
            conversation.LastMessageAt,
            conversation.LastMessage?.SenderId ?? 0,
            conversation.LastMessage?.IsRead ?? false,
            conversation.LastMessage?.MessageContent ?? string.Empty,
            conversation.Participants
        );

        return conversationResult is not null ? conversationResult : Result.Failure<ConversationDto>(Error.Failure("Conversation.Failure", "Something went wrong during process"));
    }
}
