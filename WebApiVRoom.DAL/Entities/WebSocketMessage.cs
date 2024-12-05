using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class WebSocketMessage
    {
        public int Id { get; set; } 
        public string Sender { get; set; } 
        public string Receiver { get; set; } 
        public string Content { get; set; } 
        public DateTime SentAt { get; set; } 
    }
}
