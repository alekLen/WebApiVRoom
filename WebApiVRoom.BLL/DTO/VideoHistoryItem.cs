using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class VideoHistoryItem
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string VideoTitle { get; set; }
        public bool IsShort { get; set; }
        public byte[] Cover { get; set; }
        public string VideoUrl { get; set; }
        public string VideoDescription { get; set; } = string.Empty; 
        public string VRoomVideoUrl { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int ChannelSettingsId { get; set; }
        public string ChannelName { get; set; }
        public string Channel_URL { get; set; }
        public int TimeCode { get; set; }
        public int Duration { get; set; }
    }
}
