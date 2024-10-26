using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class VideoInfoDTO
    {
        public int Id { get; set; }
        public string ObjectID { get; set; }
        public int ChannelSettingsId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelBanner { get; set; }
        public string Tittle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int Duration { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsShort { get; set; }
        public string Cover {  get; set; }
        public bool Visibility { get; set; }
        public bool IsAgeRestriction { get; set; }//Возрастные ограничения --> true = есть, false = нет
        public bool IsCopyright { get; set; }//Авторские права --> true = есть, false = нет
        public string Audience { get; set; }//Аудитория --> children = дети, adults = взрослые, all = все

    }
}
