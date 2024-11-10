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
        public string ObjectID { get; set; }
        public ChannelSettings ChannelSettings { get; set; }
        public string Tittle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int Duration { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public string VRoomVideoUrl { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsShort { get; set; }
        public byte[] Cover { get; set; }
        public bool Visibility { get; set; }//true = public, false = private
        public bool IsAgeRestriction { get; set; }//Возрастные ограничения --> true = есть, false = нет
        public bool IsCopyright { get; set; }//Авторские права --> true = есть, false = нет
        public string Audience { get; set; }//Аудитория --> children = дети, adults = взрослые, all = все


        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<HistoryOfBrowsing> HistoryOfBrowsings { get; set; } = new List<HistoryOfBrowsing>();
        public List<CommentVideo> CommentVideos { get; set; } = new List<CommentVideo>();
        public List<PlayList>? PlayLists { get; set; } = new List<PlayList>();
        public List<PlayListVideo> PlayListVideos { get; set; } = new List<PlayListVideo>();
        public TimeSpan LastViewedPosition { get; set; }
    }
}
