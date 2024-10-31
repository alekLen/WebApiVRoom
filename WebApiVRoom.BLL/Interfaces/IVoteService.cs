using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IVoteService
    {
        Task AddVote(int postId, string clerkId, int optionId);
        Task DeleteVote(int id);
        Task<VoteDTO> GetVote(int id);
        Task<VoteDTO> GetVoteByUserAndPost(string clerkId, int postId);
        Task<List<OptionVotes>> GetAllVotesByPostIdAndOptionId(int postId);
    }
}
