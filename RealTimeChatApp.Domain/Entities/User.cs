using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Entities
{
    public class User : Entity
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? ProfilePicture { get; set; }


        public ICollection<Conversation> Conversations { get; set; } = new HashSet<Conversation>();
        public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new HashSet<ConversationParticipant>();
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
        public ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<BlockedContact> BlockedContacts { get; set; } = new HashSet<BlockedContact>();

        public ICollection<UserPresence> UserPresence { get; set; } = new HashSet<UserPresence>();



    }
}
