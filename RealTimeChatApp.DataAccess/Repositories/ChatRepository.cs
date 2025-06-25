using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Dtos;
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
        var messages = await _aspContext.Conversations.Include(x=>x.Messages).ThenInclude(x=>x.Sender).FirstOrDefaultAsync(x => x.Id == ConversationId );
        return messages.Messages.ToList();
    }


    public async Task<User?> GetNewOnlineUser(int currentUserId, int newUserId)
    {
        return _aspContext.Users
            .Where(x => x.Id == newUserId && !_aspContext.BlockedContacts.Any(b => b.BlockedUserId == newUserId && b.UserId == currentUserId))
            .FirstOrDefault();
    }

    public async Task<bool> DeleteMessage(int messageId, int userId)
    {
        var message = _aspContext.Messages.FirstOrDefault(x => x.Id == messageId && x.SenderId == userId);
        if (message == null) return false; 

        message.IsDeleted = true;
        var result = _aspContext.SaveChanges();
        return result == 1 ? true : false;
    }
}
