using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Entities
{
    public class Conversation : Entity
    {
        public bool IsGroup { get; set; }
        public int CreatedBy { get; set; }
        public string? GroupName { get; set; }
        public DateTime LastMessageAt { get; set; }

        public ICollection<ConversationParticipant> Participants { get; set; } = new HashSet<ConversationParticipant>();
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
        public User? CreatedByUser { get; set; }
    }
}
