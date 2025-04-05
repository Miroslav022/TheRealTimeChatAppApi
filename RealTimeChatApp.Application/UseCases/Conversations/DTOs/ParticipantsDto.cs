namespace RealTimeChatApp.Application.UseCases.Conversations.DTOs;

public sealed record ParticipantsDto(int id, string userName, string profilePicture, bool isBlocked)
{
}
