using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public List<CommentPost> CommentPosts { get; set; } = new List<CommentPost>();
    }
}
