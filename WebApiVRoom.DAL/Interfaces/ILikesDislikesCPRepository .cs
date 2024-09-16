using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILikesDislikesCPRepository : ISetGetRepository<LikesDislikesCP>
    {
        Task<List<LikesDislikesCP>> GetByIds(List<int> ids);
        Task<LikesDislikesCP> Get(int commentId, string userid);
    }
}
