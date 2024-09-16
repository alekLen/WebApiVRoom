using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILikesDislikesAVRepository : ISetGetRepository<LikesDislikesAV>
    {
        Task<List<LikesDislikesAV>> GetByIds(List<int> ids);
        Task<LikesDislikesAV> Get(int commentId, string userid);
    }
}
