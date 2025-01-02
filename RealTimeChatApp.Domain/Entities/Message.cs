using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Entities
{
    public class Message : Entity
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string? MessageContent { get; set; }
        public int MessageTypeId { get; set; }
        public bool IsRead { get; set; }
        public int? RepliedToMessageId { get; set; }

        public Conversation? Conversation { get; set; }
        public MessageType? MessageType { get; set; }
        public ICollection<Media> Media { get; set; } = new HashSet<Media>();
        public User? Sender { get; set; }
        public Message? RepliedToMessage { get; set; }
        public ICollection<Message> Replies { get; set; } = new HashSet<Message>();

    }
}
