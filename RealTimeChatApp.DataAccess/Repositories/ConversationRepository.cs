using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly AspContext _aspContext;

    public ConversationRepository(AspContext aspContext)
    {
        _aspContext = aspContext;
    }
    
    public async Task<bool> CreateConversation(Conversation conversation, List<ConversationParticipant> participants,List<int> ids, CancellationToken cancellationToken)
    {
        ids = ids.Distinct().ToList();
        var existingConversation = await _aspContext.Conversations.Where(c => c.Participants.Count == ids.Count && c.Participants.All(p => ids.Contains(p.UserId))).FirstOrDefaultAsync(cancellationToken);
        if(existingConversation != null)
        {
            return true;
        }
        using (var transaction = _aspContext.Database.BeginTransaction())
        {
            try
            {
                await _aspContext.Conversations.AddAsync(conversation);
                await _aspContext.ConversationParticipants.AddRangeAsync(participants);
                await _aspContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

    }

    public async Task<IReadOnlyList<ConversationDto>> GetAllUsersConversations(int id, CancellationToken cancellationToken)
    {

        //REFACTOR !!!!!!

        var conversations = await _aspContext.ConversationParticipants
        .Where(x => (x.Conversation.CreatedBy == id && !x.Conversation.Messages.Any()) || (x.UserId == id && x.Conversation.Messages.Any()))
        .Select(x => new
        {
            x.Conversation.Id,
            x.Conversation.GroupName,
            x.Conversation.IsGroup,
            x.Conversation.LastMessageAt,
            ParticipantId = x.Conversation.Participants.FirstOrDefault(p => p.UserId != id).UserId,
            Username = x.Conversation.Participants.FirstOrDefault(p => p.UserId != id).User.Username,
            LastMessage = x.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()
        })
        .Distinct()
        .ToListAsync(cancellationToken);


        var result = conversations.Select(x => new ConversationDto(
            x.Id,
            x.GroupName,
            x.IsGroup,
            x.LastMessageAt,
            x.ParticipantId,    
            x.Username,
            true,
            x.LastMessage?.SenderId ?? 0,
            x.LastMessage?.IsRead ?? false,
            x.LastMessage?.MessageContent ?? string.Empty
        )).ToList();
        return result;
    }
}
