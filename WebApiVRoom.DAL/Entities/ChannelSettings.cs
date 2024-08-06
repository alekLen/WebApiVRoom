using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class ChannelSettings
    {
        public int Id { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ChannelBanner { get; set; } = string.Empty;
        public User Owner { get; set; }
        public Language Language { get; set; }
        public Country Country { get; set; }
        public bool Notification {  get; set; }=false;
        public List<Video> Videos { get; set; } = new List<Video>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    }
}
