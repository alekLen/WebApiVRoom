using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class CommentPost
    {
        public int Id { get; set; }
        public int? UserId { get; set; } = null;
        public User User { get; set; }
        public Post Post { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public AnswerPost? AnswerPost { get; set; }
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
    }
}
