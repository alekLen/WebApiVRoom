using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int? SubscriberId { get; set; } = null;
        public User Subscriber { get; set; }
        public ChannelSettings ChannelSettings { get; set; }
        public DateTime Date {  get; set; }
    }
}
