using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class CommentPostDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int? AnswerPostId { get; set; }
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
    }
}
