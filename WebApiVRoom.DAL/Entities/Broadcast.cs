using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Broadcast
    {
        public int Id { get; set; }
        public string BroadcastId { get; set; }
        public string Title { get; set; }
        public DateTime ScheduledStartTime { get; set; }
        public DateTime ScheduledEndTime { get; set; }
        public string StreamId { get; set; }
        public string Status { get; set; }
    }
}
