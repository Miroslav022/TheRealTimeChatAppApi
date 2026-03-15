namespace RealTimeChatApp.Application.UseCases.Conversations.DTOs;

public sealed record ConversationDto(
    int id,
    string displayName,
    string displayImage,
    bool IsGroup,
    DateTime LastMessageAt,
    int SenderId,
    bool IsRead,
    string LastMessage,
    List<ParticipantsDto> Participants
);

//public record ConversationDto(
//    int Id,
//    string DisplayName,
//    string DisplayImage,
//    bool IsGroup,
//    string? LastMessageContent,
//    int? LastMessageSenderId,
//    bool LastMessageIsRead,
//    DateTime? LastMessageAt,
//    List<ParticipantsDto> Participants
//);
