using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ILikesDislikesAVService
    {
        Task<LikesDislikesAVDTO> Add(LikesDislikesAVDTO t);
        Task<LikesDislikesAVDTO> Get(int commentId, string userid);
    }
}
