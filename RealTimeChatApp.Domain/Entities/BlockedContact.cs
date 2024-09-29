using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Entities
{
    public class BlockedContact : Entity
    {
        public int UserId { get; set; }
        public int BlockedUserId { get; set; }

        public virtual User? User { get; set; }
        public virtual User? BlockedUser { get; set; }

    }
}
