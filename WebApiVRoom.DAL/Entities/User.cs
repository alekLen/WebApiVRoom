using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int Clerk_Id { get; set; }
        public ChannelSettings ChannelSettings { get; set; }
        public bool IsPremium { get; set; } 
        public int SubscriptionCount { get; set; }
        public List<Subscription> Subscriptions { get; set; }=new List<Subscription>();
        public List<PlayList> PlayLists { get; set; }= new List<PlayList>();
        public List<HistoryOfBrowsing> HistoryOfBrowsing { get; set;} = new List<HistoryOfBrowsing>();

    }
}
