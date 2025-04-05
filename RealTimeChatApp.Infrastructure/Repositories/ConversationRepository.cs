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

    public async Task<bool> CreateConversation(Conversation conversation, List<ConversationParticipant> participants, List<int> ids, CancellationToken cancellationToken)
    {
        ids = ids.Distinct().ToList();
        var existingConversation = await _aspContext.Conversations.Where(c => c.Participants.Count == ids.Count && c.Participants.All(p => ids.Contains(p.UserId))).FirstOrDefaultAsync(cancellationToken);
        if (existingConversation != null)
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

        var conversations = await _aspContext.Conversations
        .Where(x => (x.CreatedBy == id && !x.Messages.Any()) || (x.Messages.Any() && x.Participants.Any(p=>p.UserId == id)))
        .Select(x => new
        {
            x.Id,
            x.GroupName,
            x.IsGroup,
            x.LastMessageAt,
            Participant = x.Participants.Where(participant => participant.UserId != id).Select(participant => new ParticipantsDto
            (
                participant.UserId,
                participant.User.Username,
                participant.User.ProfilePicture,
                x.CreatedByUser != null && x.CreatedByUser.BlockedContacts.Any(bc => bc.BlockedUserId == participant.UserId)
            )).FirstOrDefault(),
            LastMessage = x.Messages.OrderByDescending(m=>m.CreatedAt).Take(1).FirstOrDefault()
        })
        .ToListAsync(cancellationToken);

        var result = conversations.Select(x => new ConversationDto(
            x.Id,
            x.GroupName,
            x.IsGroup,
            x.LastMessageAt,
            true,
            x.LastMessage?.SenderId ?? 0,
            x.LastMessage?.IsRead ?? false,
            x.LastMessage?.MessageContent ?? string.Empty,
            x.Participant
        )).ToList();
        return result;
    }
}
