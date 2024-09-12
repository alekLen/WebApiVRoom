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
        public string Clerk_Id { get; set; }
        public int ChannelSettings_Id { get; set; }
        public string ChannelName { get; set; }
        public string ChannelBanner { get; set; }
        public bool IsPremium { get; set; }
        public int SubscriptionCount { get; set; }
        public List<int> Subscriptions { get; set; } = new List<int>();
        public List<int> PlayLists { get; set; } = new List<int>();
        public List<int> HistoryOfBrowsing { get; set; } = new List<int>();
        public List<int> CommentPosts { get; set; } = new List<int>();
        public List<int> CommentVideos { get; set; } = new List<int>();
        public List<int> AnswerPosts { get; set; } = new List<int>();
        public List<int> AnswerVideos { get; set; } = new List<int>();

    }
}
