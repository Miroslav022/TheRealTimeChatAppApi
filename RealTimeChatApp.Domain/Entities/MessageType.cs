using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain.Entities
{
    public class MessageType : Entity
    {
        public string? Name { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Media> Medias { get; set; } = new List<Media>();
    }
}
