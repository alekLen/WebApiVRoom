using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILikesDislikesVRepository : ISetGetRepository<LikesDislikesV>
    {
        Task<List<LikesDislikesV>> GetByIds(List<int> ids);
        Task<LikesDislikesV> Get(int commentId, string userid);
        Task<List<LikesDislikesV>>  GetLikedVideoByUserId(string userid);
    }
}
