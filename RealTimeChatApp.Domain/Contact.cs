using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain
{
    public class Contact : Entity
    {
        public int UserId { get; set; }
        public int ContactUserId { get; set; }

        public User? User { get; set; } 
        public User? ContactUser { get; set; }
    }
}
