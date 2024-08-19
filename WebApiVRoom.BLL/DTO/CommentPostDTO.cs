using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public sealed class CommentPostDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; } = null; 
        public int PostId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int? AnswerPostId { get; set; } = null;
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
    }
}
