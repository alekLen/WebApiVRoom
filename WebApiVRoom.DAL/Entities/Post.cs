using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.DAL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }    
        public ChannelSettings ChannelSettings { get; set; }
        public DateTime Date { get; set; }
        public string? Photo { get; set; }
        public string? Video { get; set; }
        public string? Type { get; set; }
        public List<OptionsForPost>? Options { get; set; } = new List<OptionsForPost>();
        public List<Vote>? Voutes { get; set; } = new List<Vote>();
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public List<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();
    }
}
