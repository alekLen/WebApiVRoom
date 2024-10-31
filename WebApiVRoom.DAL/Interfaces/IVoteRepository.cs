using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IVoteRepository : ISetGetRepository<Vote>
    {
        Task<Vote> GetVoteByUserAndPost(string clerkId, int postId);
        Task<Vote> GetByPost(int postId);
        Task<IEnumerable<Vote>> GetAllPaginated(int pageNumber, int pageSize);
        Task<List<Vote>> GetByIds(List<int> ids);
        Task<List<Vote>> GetByPostIdAndOptionId(int postId, int opId);
    }
}
