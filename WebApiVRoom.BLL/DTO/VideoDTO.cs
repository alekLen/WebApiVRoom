using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string? ObjectID { get; set; } = string.Empty;
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
        public string Cover { get; set; }
        public bool Visibility { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<int> TagIds { get; set; } = new List<int>();
        public List<int> HistoryOfBrowsingIds { get; set; } = new List<int>();
        public List<int> CommentVideoIds { get; set; } = new List<int>();
        public List<int> PlayLists { get; set; } = new List<int>();
        public string LastViewedPosition { get; set; }
    }
}
