namespace RealTimeChatApp.Application.UseCases.Conversations.DTOs;

public sealed record ConversationDto(int id, string GroupName, bool IsGroup, DateTime LastMessageAt, int UserId, string username, bool isAdmin, int SenderId, bool IsRead, string LastMessage)
{
}
