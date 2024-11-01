using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.DTO
{
    public class VoteDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
       public int OptionId { get; set; }
    }

    public class OptionVotes
    {
        public int Index { get; set; }
        public int AllCounts { get; set; }
    }
    public class OptionVotesResponse
    {
        public int Index { get; set; }
        public int AllCounts { get; set; }
        public double Rate { get; set; }
    }

    public class VotesForResponse
    {
        public bool IsVoted {  get; set; }
        public int AllVotes { get; set; }
        public List<OptionVotesResponse> Options { get; set; }
    }
}
