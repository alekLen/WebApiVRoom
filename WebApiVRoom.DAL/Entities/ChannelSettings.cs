﻿using System;
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
        public string ChannelPlofilePhoto { get; set; } = string.Empty;
        public string Channel_URL { get; set; } = string.Empty;
        public string? ChannelNikName { get; set; } = string.Empty;
        public List<ChannelSection> ChannelSections { get; set; } = new List<ChannelSection>();//
        public PinnedVideo? PinnedVideo { get; set; }
        public User Owner { get; set; }
        public Language Language { get; set; }
        public Country Country { get; set; }
        public bool Notification {  get; set; }=false;
        public List<Video> Videos { get; set; } = new List<Video>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public int SubscriptionCount { get; set; } = 0;
        public List<PlayList> PlayLists { get; set; } = new List<PlayList>();
        public List<HistoryOfBrowsing> HistoryOfBrowsing { get; set; } = new List<HistoryOfBrowsing>();
        public List<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();
        public List<CommentVideo> CommentVideos { get; set; } = new List<CommentVideo>();
        public List<AnswerPost> AnswerPosts { get; set; } = new List<AnswerPost>();
        public List<AnswerVideo> AnswerVideos { get; set; } = new List<AnswerVideo>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();


        public static explicit operator ChannelSettings(Task<ChannelSettings> v)
        {
            throw new NotImplementedException();
        }
    }
}
