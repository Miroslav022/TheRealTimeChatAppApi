using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Entities
{
    public class Notification : Entity
    {
        public int UserId { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }

        public User? User { get; set; }


    }
}
