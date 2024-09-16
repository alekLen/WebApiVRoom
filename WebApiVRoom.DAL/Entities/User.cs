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
        public string Clerk_Id { get; set; } = string.Empty;
        public int ChannelSettings_Id { get; set; }
        //public string? ChannelName { get; set; }
        //public string ChannelBanner { get; set; }
        public bool IsPremium { get; set; } = false;
        //public int SubscriptionCount { get; set; } = 0;
        //public List<Subscription> Subscriptions { get; set; } =new List<Subscription>();
        //public List<PlayList> PlayLists { get; set; } = new List<PlayList>();
        //public List<HistoryOfBrowsing> HistoryOfBrowsing { get; set;} = new List<HistoryOfBrowsing>();
        //public List<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();
        //public List<CommentVideo> CommentVideos { get; set; } = new List<CommentVideo>();
        //public List<AnswerPost> AnswerPosts { get; set; } = new List<AnswerPost>();
        //public List<AnswerVideo> AnswerVideos { get; set; } = new List<AnswerVideo>();
        //public List<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
