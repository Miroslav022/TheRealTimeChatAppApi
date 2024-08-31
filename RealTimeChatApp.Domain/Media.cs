using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Domain
{
    public class Media : Entity
    {
        public int MessageId { get; set; }
        public string? Url { get; set; }
        public int MediaTypeId { get; set; }

        public Message? Message { get; set; }
        public MessageType? MediaType { get; set; }

        


    }
}
