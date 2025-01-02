using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class WebRTCSession
    {
        public int Id { get; set; }
        public string SessionId { get; set; } 
        public string HostUserId { get; set; } 
        public string GuestUserId { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public List<WebRTCConnection> Connections { get; set; } = new List<WebRTCConnection>();
    }
}
