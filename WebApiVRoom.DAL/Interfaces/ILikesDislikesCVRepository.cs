using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILikesDislikesCVRepository : ISetGetRepository<LikesDislikesCV>
    {
        Task<List<LikesDislikesCV>> GetByIds(List<int> ids);
        Task<LikesDislikesCV> Get(int commentId, string userid);
    }
}
