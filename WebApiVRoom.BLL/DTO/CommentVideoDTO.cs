using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public sealed class CommentVideoDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }                      
        public int VideoId { get; set; }
        public string UserName { get; set; }
        public string ChannelBanner { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int? AnswerVideoId { get; set; }
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
        public List<string>? users { get; set; }=new List<string>();
    }
}
