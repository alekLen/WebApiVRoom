using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class LikesDislikesAP
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public AnswerPost answerPost { get; set; }
    }
}
