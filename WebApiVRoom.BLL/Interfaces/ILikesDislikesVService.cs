using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
   public interface ILikesDislikesVService
    {
        Task<LikesDislikesVDTO> Add(LikesDislikesVDTO t);
        Task<LikesDislikesVDTO> Get(int commentId, string userid);
    }
}
