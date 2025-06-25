using RealTimeChatApp.Application.Abstractions.Query;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Messages.DTOs;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.UseCases.Messages.Query;

public class MessageQueryHandler : IQueryHandler<MessageQuery, List<MessagesDto>>
{
    private readonly IChatRepository _chatRepository;
    public MessageQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    public async Task<Result<List<MessagesDto>>> Handle(MessageQuery request, CancellationToken cancellationToken)
    {
        var messagesResult = await _chatRepository.GetMessages(request.conversationId);

        if (!messagesResult.IsSuccess || messagesResult.Value == null || !messagesResult.Value.Any())
        {
            return (Result<List<MessagesDto>>)Result<List<MessagesDto>>.Failure(Error.NotFound("NoMessages", "There are no messages in store"));
        }

        var message = messagesResult.Value.Where(x=>!x.IsDeleted).Select(x =>
        new MessagesDto
        {
            id = x.Id,
            SenderId = x.SenderId,
            SenderUserName = x.Sender.Username,
            MessageContent = x.MessageContent,
            MessageType = x.MessageTypeId,
            IsRead = x.IsRead,
            RepliedToMessageId = x.RepliedToMessageId,
            RepliedToMessage = new RepliedToMessage { SenderUserName = x.RepliedToMessage?.Sender?.Username, MessageContent = x?.RepliedToMessage?.MessageContent},
            CreatedAt = x.CreatedAt,
            IsDeleteed = x.IsDeleted
        }
        ).ToList();

        return Result.Success(message);
    }
}
