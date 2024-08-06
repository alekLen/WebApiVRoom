using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class CommentVideo
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Video Video { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public AnswerVideo AnswerVideo { get; set; }
        public bool IsPinned { get; set; }
        public bool IsEdited { get; set; }
    }
}
