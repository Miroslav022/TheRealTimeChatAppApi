namespace RealTimeChatApp.Application.UseCases.Conversations.DTOs;

public sealed record ConversationDto(int id, string GroupName, bool IsGroup, DateTime LastMessageAt, bool isAdmin, int SenderId, bool IsRead, string LastMessage, ParticipantsDto Participant)
{
}
