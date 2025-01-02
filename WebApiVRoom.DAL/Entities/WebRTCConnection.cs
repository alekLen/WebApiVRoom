using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class WebRTCConnection
    {
        public int Id { get; set; }
        public int WebRTCSessionId { get; set; }
        public WebRTCSession WebRTCSession { get; set; }
        public string ConnectionId { get; set; } 
        public string SDP { get; set; } 
        public string ICECandidates { get; set; } 
        public DateTime LastUpdated { get; set; }
    }

}
