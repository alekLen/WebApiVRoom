using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ChannelSettingsId { get; set; }
        public DateTime Date { get; set; }
        public string? Photo { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
      //  public List<int> CommentPostsId { get; set; } = new List<int>();
    }
}
