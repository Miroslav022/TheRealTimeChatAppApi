using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly AspContext _aspContext;
    private readonly IUnitOfWork _unitOfWork;

    public ConversationRepository(AspContext aspContext, IUnitOfWork unitOfWork)
    {
        _aspContext = aspContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Conversation> CreateConversation(int createdById, List<int> participantIds, CancellationToken cancellationToken)
    {
        participantIds = participantIds.Distinct().ToList();
        var existingConversation = await _aspContext.Conversations.Include(x => x.Participants).ThenInclude(x => x.User).Where(c => c.Participants.Count == participantIds.Count && c.Participants.All(p => participantIds.Contains(p.UserId))).FirstOrDefaultAsync(cancellationToken);
        if (existingConversation != null)
        {
            return existingConversation;
        }
        using (var transaction = _aspContext.Database.BeginTransaction())
        {
            try
            {
                Conversation conversation = new Conversation
                {
                    CreatedBy = createdById,
                    LastMessageAt = DateTime.Now,
                };

                var users = _aspContext.Users.Where(x => participantIds.Contains(x.Id)).ToDictionary(x => x.Id);

                var participants = participantIds.Select(id => new ConversationParticipant
                {
                    Conversation = conversation,
                    UserId = id,
                    IsAdmin = id == createdById ? true : false,
                    User = users[id]
                });

                await _aspContext.Conversations.AddAsync(conversation);
                await _aspContext.ConversationParticipants.AddRangeAsync(participants);
                await _aspContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return conversation;
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

        var conversationsRaw = await _aspContext.Conversations
            .AsNoTracking()
            .Where(x =>
                (x.CreatedBy == id && !x.Messages.Any()) ||
                ((x.Messages.Any() || x.IsGroup) && x.Participants.Any(p => p.UserId == id)))
            .Select(x => new
            {
                x.Id,
                x.GroupName,
                x.IsGroup,
                x.LastMessageAt,
                x.CreatedByUser,
                Participants = x.Participants.Select(participant => new
                {
                    participant.UserId,
                    participant.User.Username,
                    participant.User.ProfilePicture,
                }),
                LastMessage = x.Messages.Where(m => !m.IsDeleted).OrderByDescending(m => m.CreatedAt).FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        var blockedIds = await _aspContext.BlockedContacts
            .Where(x => x.UserId == id)
            .Select(bc => bc.BlockedUserId)
            .ToListAsync(cancellationToken);

        var result = conversationsRaw.Select(x =>
        {
            var participants = x.Participants.Where(p => p.UserId != id)
            .Select(p => new ParticipantsDto
            (
                p.UserId,
                p.Username,
                p.ProfilePicture,
                blockedIds.Contains(p.UserId)
            )).ToList();

            var otherUser = participants.FirstOrDefault();
            var displayName = x.IsGroup ? x.GroupName : otherUser?.userName;
            var displayPicture = x.IsGroup ? "groupDefault.png" : otherUser?.profilePicture;
            var isRead = x.LastMessage is not null && x.LastMessage.SenderId != id ? x.LastMessage.IsRead : true;

            return new ConversationDto
            (
                x.Id,
                displayName,
                displayPicture,
                x.IsGroup,
                x.LastMessageAt,
                x.LastMessage?.SenderId ?? 0,
                isRead,
                x.LastMessage?.MessageContent ?? string.Empty,
                participants
            );
        }).ToList();
        return result;
    }

    public async Task<IReadOnlyCollection<Conversation>> getAllUserConversationsWithoutGroups(int id, CancellationToken cancellationToken = default)
    {
        return await _aspContext.ConversationParticipants.Where(x => x.UserId == id && !x.Conversation.IsGroup).Include(x => x.Conversation.Participants).Select(x => x.Conversation).ToListAsync(cancellationToken);
    }

    public async Task Insert(Conversation conversation, CancellationToken cancellationToken)
    {
        _aspContext.Conversations.Add(conversation);
        await _unitOfWork.SaveChangesAsync();
    }
}
