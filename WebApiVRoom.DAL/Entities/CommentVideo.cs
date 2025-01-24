using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.DAL.Entities
{

    public class CommentVideo
    {
        public int Id { get; set; }
        public string clerkId { get; set; } = null;
        public ChannelSettings User { get; set; }
        
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Video Video { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
        public List<LikesDislikesCV> users { get; set; } = new List<LikesDislikesCV>();
    }

}
