using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public int Clerk_Id { get; set; }
        public int ChannelSettings_Id { get; set; }
        public bool IsPremium { get; set; }
        public int SubscriptionCount { get; set; }
        public List<int> Subscriptions_Id { get; set; } = new List<int>();
        public List<int> PlayLists_Id { get; set; } = new List<int>();
        public List<int> HistoryOfBrowsings_Id { get; set; } = new List<int>();
    }
}
