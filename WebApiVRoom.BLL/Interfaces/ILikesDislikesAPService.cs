using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ILikesDislikesAPService
    {
        Task<LikesDislikesAPDTO> Add(LikesDislikesAPDTO t);
        Task<LikesDislikesAPDTO> Get(int commentId, string userid);
    }
}
