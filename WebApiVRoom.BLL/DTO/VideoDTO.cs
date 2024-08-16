using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public int ChannelSettingsId { get; set; }
        public string Tittle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int Duration { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsShort { get; set; }
        public List<int> CategoriesId { get; set; } = new List<int>();
        public List<int> TagsId { get; set; } = new List<int>();
        public List<int> HistoryOfBrowsingsId { get; set; } = new List<int>();
        public List<int> CommentVideosId { get; set; } = new List<int>();
    }
}
