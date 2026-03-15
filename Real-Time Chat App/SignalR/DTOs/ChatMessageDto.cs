namespace Real_Time_Chat_App.SignalR.DTOs;

public record ChatMessageDto(int conversationId, int senderId, List<string> participantIds, string message, int messageTypeId, int? replyToMessageId)
{
}
