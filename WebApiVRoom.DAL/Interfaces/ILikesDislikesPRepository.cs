using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILikesDislikesPRepository : ISetGetRepository<LikesDislikesP>
    {
        Task<List<LikesDislikesP>> GetByIds(List<int> ids);
        Task<LikesDislikesP> Get(int commentId, string userid);
    }
}
