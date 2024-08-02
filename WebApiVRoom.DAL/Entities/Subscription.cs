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
        public User Subscriber { get; set; }
        public ChannelSettings Channel { get; set; }
        public DateTime Date {  get; set; }
    }
}
