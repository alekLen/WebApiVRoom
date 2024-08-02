using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public User Uploader { get; set; } 
        public string Tittle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int Duration { get; set; }
        public string VideoUrl { get; set; }= string.Empty;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsShort { get; set; }
        public List<Category> Categories { get; set; }=new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
