public class MessageStatus : Entity
{
    public int MessageId { get; set; }
    public int UserId { get; set; }
    public MessageStatusType Status { get; set; }
    public DateTime Timestamp { get; set; }

    public Message Message { get; set; }
    public User User { get; set; }
}

public enum MessageStatusType
{
    Sent = 0,
    Received = 1,
    Seen = 2
}
