using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Video
    {
        public long Id { get; set; }
        public long UploaderId { get; set; }
        public string Tittle { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public long Duration { get; set; }
        public string VideoUrl { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string Tags {  get; set; }
        public long CategoryId { get; set; }
        public int BrowsingCount { get; set; }

    }
}
