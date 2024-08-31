using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain
{
    public class UserPresence : Entity
    {
        public int UserId { get; set; }
        public DateTime LastSeenAt { get; set; }
        public bool IsOnline { get; set; }

        public User? User { get; set; }
    }
}
