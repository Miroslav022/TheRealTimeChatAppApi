using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Messages.DTOs;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AspContext _aspContext;

    public ChatRepository(AspContext aspContext)
    {
        _aspContext = aspContext;
    }

    public async Task SaveMessageAsync(Message message)
    {
        _aspContext.Messages.Add(message);
        await _aspContext.SaveChangesAsync();
    }

    public async Task<Result<IReadOnlyList<Message>>> GetMessages(int ConversationId)
    {
        var messages = await _aspContext.Conversations.Include(x=>x.Messages).ThenInclude(x=>x.Sender).FirstOrDefaultAsync(x => x.Id == ConversationId);
        return messages.Messages.ToList();
    }
}
