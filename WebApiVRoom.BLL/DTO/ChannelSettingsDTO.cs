using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class ChannelSettingsDTO
    {
        public int Id { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ChannelBanner { get; set; } = string.Empty;
        public string ChannelProfilePhoto { get; set; } = string.Empty;
        public string Channel_URL { get; set; } = string.Empty;
        public string? ChannelNikName { get; set; } = string.Empty;
        public int SubscriptionCount { get; set; } = 0;
        public int Owner_Id { get; set; }
        public int Language_Id { get; set; }
        public int Country_Id { get; set; }
        public bool Notification { get; set; } = false;
        public List<int> Videos { get; set; } = new List<int>();
        public List<int> Posts { get; set; } = new List<int>();
        public List<int> Subscriptions { get; set; } = new List<int>();

    }
}
