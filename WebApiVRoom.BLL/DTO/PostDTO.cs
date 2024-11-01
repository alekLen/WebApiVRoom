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
        public string? Video { get; set; }
        public string? Type { get; set; }
        public List<string>? Options { get; set; } = new List<string>();
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
  
    }
}
