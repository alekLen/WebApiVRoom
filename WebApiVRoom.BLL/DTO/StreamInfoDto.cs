using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class StreamInfoDto
    {
        public string StreamId { get; set; }
        public string HostUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ViewerCount { get; set; }
    }
}
