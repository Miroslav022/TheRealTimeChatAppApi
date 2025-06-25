namespace RealTimeChatApp.Application.UseCases.Messages.DTOs;

public class MessagesDto
{
    public int id { get; set; }
    public int SenderId { get; set; }
    public string SenderUserName { get; set; }
    public string MessageContent { get; set; }
    public int MessageType { get; set; }
    public bool IsRead { get; set; }
    public int? RepliedToMessageId { get; set; }
    public RepliedToMessage RepliedToMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleteed { get; set; }
}

public class RepliedToMessage
{
    public string SenderUserName { get; set; }
    public string MessageContent { get; set; }
}