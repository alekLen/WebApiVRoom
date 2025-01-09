using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class WebRTCConnectionModel
    {
        public string SessionId { get; set; }
        public string ConnectionId { get; set; }
        public string SDP { get; set; }
        public string ICECandidates { get; set; }
    }

}
