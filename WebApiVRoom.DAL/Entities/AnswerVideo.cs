using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class AnswerVideo
    {
        public int Id { get; set; }
        //public User User { get; set; }
   public string clerkId { get; set; } = null;
        public ChannelSettings User { get; set; }
        public int CommentVideo_Id { get; set; }

        public string Text {  get; set; }
        public DateTime AnswerDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsEdited { get; set; }
    }
}
