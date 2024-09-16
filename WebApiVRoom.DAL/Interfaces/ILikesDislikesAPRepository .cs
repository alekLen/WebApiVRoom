using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILikesDislikesAPRepository : ISetGetRepository<LikesDislikesAP>
    {
        Task<List<LikesDislikesAP>> GetByIds(List<int> ids);
        Task<LikesDislikesAP> Get(int commentId, string userid);
    }
}
