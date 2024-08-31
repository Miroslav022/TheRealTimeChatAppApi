using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain
{
    public class ConversationParticipant : Entity
    {
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }

        public User? User { get; set; }
        public Conversation? Conversation { get; set; }
    }
}
