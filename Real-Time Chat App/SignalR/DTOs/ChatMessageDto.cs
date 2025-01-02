namespace Real_Time_Chat_App.SignalR.DTOs;

public record ChatMessageDto(int conversationId, int senderId, int receiverId, string message, int messageTypeId)
{
}
