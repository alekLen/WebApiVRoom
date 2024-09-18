using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ILikesDislikesPService
    {
        Task<LikesDislikesPDTO> Add(LikesDislikesPDTO t);
        Task<LikesDislikesPDTO> Get(int commentId, string userid);
    }
}
